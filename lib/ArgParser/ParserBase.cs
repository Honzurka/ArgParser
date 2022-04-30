using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ArgParser
{
	/// <summary>
	/// A base class which parses commandline arguments.
	/// 
	/// To specify options / arguments, inherit from this class and declare
	/// option / argument fields that are descendants of
	/// <see cref="ArgParser.ArgumentBase{T}">ArgumentBase</see> /
	/// <see cref="OptionBase{T}">OptionBase</see>.
	/// After the <see cref="Parse(string[])">Parse</see> method is called,
	/// parsed results can be obtained using fieldName.GetValue().
	/// </summary>
	public abstract class ParserBase
	{
		readonly List<IOption> options;

		protected ParserBase()
		{
			List<IOption> GetOptions()
			{
				List<IOption> result = new();

				var fields = GetType().GetFields();
				foreach (var field in fields)
				{
					if (field.FieldType.GetInterface(nameof(IOption)) != null)
					{
						result.Add((IOption)(field.GetValue(this))!);
					}
				}

				return result;
			}

			void CheckThatOptionsHaveAtLeastOneAlias()
			{
				foreach (var opt in options)
				{
					if (opt.Names.Length == 0)
						throw new ArgumentException("Each option must have at least 1 name.");
				}
			}

			void CheckConflictingAliases()
			{
				HashSet<string> aliases = new();

				foreach (var opt in options)
				{
					foreach (var name in opt.Names)
					{
						if (!aliases.Contains(name))
							aliases.Add(name);
						else
							throw new ParserCodeException($"Parameter aliases `{name}` are in conflict.");
					}
				}
			}

			void CheckUnknownPlainArguments()
			{
				void CheckDuplicates(IArgument[] orderedPlainArgs) {
					var duplicates = orderedPlainArgs.GroupBy(x => x).Where(g => g.Count() > 1);
					if (duplicates.Any())
					{
						throw new ParserCodeException($"Duplicate elements `{string.Join(' ', duplicates.Select(d => d.Key.Name))}` in argument order specification");
					}
				}

				void CheckArgOrder(IEnumerable<IArgument> minuend, IEnumerable<IArgument> subtrahend, string errorMessage)
				{
					var diff = minuend.Except(subtrahend);
					if (diff.Any())
					{
						throw new ParserCodeException($"{errorMessage}: `{diff.First().Name}`");
					}
				}
				
				var orderedPlainArgs = GetArgumentOrder();
				var classPlainArgs = GetType().GetFields()
					.Where(field => field.FieldType.GetInterface(nameof(IArgument)) != null)
					.Select(field => (IArgument)(field.GetValue(this))!);

				CheckDuplicates(orderedPlainArgs);
				CheckArgOrder(orderedPlainArgs, classPlainArgs, "Unused plain argument out of argument order");
				CheckArgOrder(classPlainArgs, orderedPlainArgs, "Unknown plain argument in argument order ( didn't you forget to override GetArgumentOrder? :) )");
			}

			void CheckMultipleVariadicPlainArgs()
			{
				var variadicArgs = GetArgumentOrder().Where(a => a.ParameterAccept.IsVariadic);

				if (variadicArgs.Count() > 1)
				{
					throw new ParserCodeException(
						$"Multiple plain arguments with variadic count:" +
						$"`{string.Join(' ', variadicArgs.Select(a => a.Name))}`");
				}
			}

			options = GetOptions();
			CheckThatOptionsHaveAtLeastOneAlias();
			CheckConflictingAliases();
			CheckUnknownPlainArguments();
			CheckMultipleVariadicPlainArgs();
		}

		/// <summary>
		/// A command-line argument indicating the end of options.
		/// 
		/// When passed to the commandline args, any subsequent argument is
		/// considered to be a plain argument. By default the delimiter
		/// consists of 2 dashes ("--").
		/// </summary>
		protected virtual string Delimiter => "--";

		/// <summary>
		/// Used by the parser to determine order of arguments.
		///
		/// Since reflection doesn't necessarily retain the order of fields,
		/// the argument order needs to be specified by the user, by making
		/// an array of references to the class' arguments.
		/// 
		/// The argument order should not contain unrecognised IArgument
		/// instances and at most one plain argument can be variadic.
		/// </summary>
		/// <returns>References to argument fields.</returns>
		protected virtual IArgument[] GetArgumentOrder() =>
			Array.Empty<IArgument>();

		/// <summary>
		/// Parses args and stores parsed values in declared fields.
		/// 
		/// The parser splits the arguments into options / plain arguments
		/// (all options precede plain arguments), and passes values to
		/// corresponding options / plain arguments, according to the
		/// <see cref="OptionBase.parameterAccept">options'</see> /
		/// <see cref="ArgumentBase.parameterAccept">arguments'</see>
		/// ParameterAccept fields.
		/// </summary>
		/// <param name="args">Arguments to parse</param>
		/// <exception cref="ParserCodeException">
		/// Thrown when the class doesn't conform to the parser requirements.
		/// </exception>
		/// <exception cref="ParseException">
		/// Thrown when parsed arguments don't satisfy declared option fields:
		/// - Fields not inheriting from predefined classes
		///   (descendants of `OptionBase<T>` or `ArgumentBase<T>`)
		/// - Defining argument fields without overriding `GetArgumentOrder`
		/// </exception>
		public void Parse(string[] args)
		{
			IOption? TryGetOption(string name) => options.Find(o => o.MatchingOptionName(name));

			/// <summary>
			/// Returns value between option.ParameterAccept.MinParamAmount and
			/// option.ParameterAccept.MaxParamAmount. Consume as much params
			/// as possible if variadic.
			/// </summary>
			int GetOptionParamCount(IOption option, string[] args, int argIdx)
			{
				int result = 0;
				for (; result < option.ParameterAccept.MaxParamAmount; result++)
				{
					int idx = argIdx + result + 1;
					if (idx == args.Length || args[idx] == Delimiter || TryGetOption(args[idx]) != null)
					{
						if (result < option.ParameterAccept.MinParamAmount)
							throw new ParseException($"Too few parameters passed to option `{option.Names.First()}`");
						else break;
					}
				}
				return result;
			}

			/// <returns>
			/// Remaining plain arguments, filtered of the parsed options
			/// </returns>
			List<string> ParseOptions(string[] args)
			{
				List<string> nonOptions = new ();
				bool IsPlainArg(string arg) => !arg.StartsWith("-");

				int argIdx = 0;
				while (argIdx < args.Length)
				{
					var currentArg = args[argIdx];

					if (IsPlainArg(currentArg))
					{
						nonOptions.Add(currentArg);
						argIdx++;
						continue;
					}
					else if (currentArg == Delimiter)
					{
						argIdx++;
						nonOptions.AddRange(args[argIdx..]);
						break;
					}

					var option = TryGetOption(currentArg);
					if (option == null) throw new ParseException($"Unrecognized option `{currentArg}`");

					var paramCount = GetOptionParamCount(option, args, argIdx);
					option.CallParse(args[(argIdx + 1)..(argIdx + paramCount + 1)]);
					argIdx += paramCount + 1;
				}

				return nonOptions;
			}

			void CheckPlainParamCounts(IArgument[] orderedArguments, int argsLength)
			{
				long min = orderedArguments
				   .Select(a => a.ParameterAccept.MinParamAmount)
				   .Aggregate(0L, (acc, val) => acc + val);

				long max = orderedArguments
				   .Select(a => a.ParameterAccept.MaxParamAmount)
				   .Aggregate(0L, (acc, val) => acc + val);

				if (argsLength < min) throw new ParseException($"Insufficient amount of arguments (min {min})");
				if (argsLength > max) throw new ParseException($"Too many arguments (max {max})");
			}

			int GetPlainParamCount(int plainArgIdx, int argsCount)
			{
				var OrderedArguments = GetArgumentOrder();
				var currentArg = OrderedArguments[plainArgIdx];

				if (!currentArg.ParameterAccept.IsVariadic)
					return currentArg.ParameterAccept.MinParamAmount;

				var argCountRequiredByFollowingArgs = OrderedArguments[(plainArgIdx + 1)..]
					.Select(a => a.ParameterAccept.MinParamAmount)
					.Aggregate(0, (acc, val) => acc + val);

				return argsCount - argCountRequiredByFollowingArgs;
			}

			void ParsePlainArgs(List<string> args, IArgument[] orderedArguments)
			{
				int argsIdx = 0;
				for (int plainArgIdx = 0; plainArgIdx < orderedArguments.Length; plainArgIdx++)
				{
					var remainingArgumentsCount = args.Count - argsIdx;
					var currentArg = orderedArguments[plainArgIdx];
					var valCount = GetPlainParamCount(plainArgIdx, remainingArgumentsCount);

					var plainArgValues = args.GetRange(argsIdx, valCount);
					currentArg.CallParse(plainArgValues.ToArray());
					argsIdx += valCount;
				}
			}

			void CheckThatMandatoryOptionsAreParsed()
			{
				foreach (var o in options)
				{
					if (o.IsMandatory && !o.IsSet)
						throw new ParseException($"Mandatory option `{o.Names.First()}` not set");
				}
			}

			var filteredPlainArgs = ParseOptions(args);
			var orderedArguments = GetArgumentOrder();
			CheckPlainParamCounts(orderedArguments, filteredPlainArgs.Count);
			ParsePlainArgs(filteredPlainArgs, orderedArguments);
			CheckThatMandatoryOptionsAreParsed();
		}

		/// <summary>
		/// Generates help message from declared fields (name, description).
		/// 
		/// The function scans all the parser's
		/// <see cref="OptionBase">options</see> /
		/// <see cref="ArgumentBase">arguments</see> and based on their names
		/// and descriptions, but also their parameterAccept and validation
		/// constraints, produces a readable helptext for each one,
		/// concatenated into one string.
		/// </summary>
		public string GenerateHelp()
		{
			string GetArgumentName(IArgument argument)
			{
				string result = argument.Name;

				if (argument.ParameterAccept.MaxParamAmount > 1)
					result += "...";
				if (argument.ParameterAccept.MinParamAmount == 0)
					result = $"[{result}]";

				return result;
			}

			string GetUsageExampleLine()
			{
				string programName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
				string result = programName;

				if (options.Count > 0)
				{
					result += " [options]";
				}

				foreach (var plainArg in GetArgumentOrder())
					result += $" {GetArgumentName(plainArg)}";
				result += "\n\n";

				return result;
			}

			string GetOptionHelp()
			{
				string result = "Options:\n";
				foreach (var opt in options)
					result += $"{opt.GetHelp()}";
				
				return result;
			}

			string GetPlainArgHelp()
			{
				string result = "Arguments:\n";
				foreach (var plainArg in GetArgumentOrder())
					result += plainArg.GetHelp();
				return result;
			}

			StringBuilder result = new();
			result.Append(GetUsageExampleLine());
			result.Append(GetOptionHelp());
			result.Append(GetPlainArgHelp());
			return result.ToString();
		}
	}
}
