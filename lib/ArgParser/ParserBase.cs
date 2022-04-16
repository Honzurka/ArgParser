using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace ArgParser
{
    /// <summary>
    /// To specify options/arguments
    /// Inherit from this class and declare Option / Argument fields that are descendants of
    /// <see cref="ArgParser.ArgumentBase">ArgumentBase</see> / <see cref="ArgParser.OptionBase{T}">OptionBase</see>
    /// After the <see cref="ArgParser.ParserBase.Parse(string[])">Parse</see> method is called,
    /// parsed results could be obtained using fieldName.GetValue()
    /// </summary>
    public abstract class ParserBase
    {
        List<IOption> options;

        protected ParserBase()
        {
            void AssignOptions()
            {
                options = new List<IOption>();
                var fields = GetType().GetFields();
                foreach (var field in fields)
                {
                    if (field.FieldType.GetInterface(nameof(IOption)) != null)
                    {
                        options.Add((IOption)field.GetValue(this));
                    }
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
                            throw new ParserCodeException("Parameter aliases are in conflict.");
                    }
                }

            }

            void CheckUnknownPlainArguments()
            {
                var orderedPlainArgs = GetArgumentOrder();
                var classPlainArgs = GetType().GetFields()
                    .Where(field => field.FieldType.GetInterface(nameof(IArgument)) != null)
                    .Select(field => (IArgument)field.GetValue(this));

                var orderedPlainArgsSet = orderedPlainArgs.ToHashSet();
                var classPlainArgsSet = classPlainArgs.ToHashSet();

                if (orderedPlainArgs.Length != orderedPlainArgsSet.Count)
                    throw new ParserCodeException("Duplicate elements in argument order specification");

                foreach (var pa in orderedPlainArgsSet)
                    if (!classPlainArgsSet.Contains(pa))
                        throw new ParserCodeException("Unused plain arguments out of argument order");

                foreach (var pa in classPlainArgsSet)
                    if (!orderedPlainArgsSet.Contains(pa))
                        throw new ParserCodeException("Unknown plain arguments in argument order ( didn't you forget to override it? :) )");
            }

            void CheckMultipleVariadicPlainArgs()
            {
                if (GetArgumentOrder().Where(argument => argument.ParameterAccept.IsVariadic).Count() > 1)
                    throw new ParserCodeException("Multiple plain arguments with variadic count");
            }

            AssignOptions();
            CheckConflictingAliases();
            CheckUnknownPlainArguments();
            CheckMultipleVariadicPlainArgs();
        }



        /// <summary>
        /// A command-line argument consisting of two dashes only, default: "--". Any subsequent argument is considered to be a plain argument
        /// </summary>
        protected virtual string Delimiter => "--";

        /// <summary>
        /// Used by the parser to determine order of arguments.
        /// Since reflection doesn't necessarily retain the order of fields,
        /// it needs to be specified by the user.
        /// </summary>
        /// <returns>References to argument fields</returns>
        protected virtual IArgument[] GetArgumentOrder() => Array.Empty<IArgument>();
        internal IArgument[] CallGetArgumentOrder() => GetArgumentOrder();
        
        /// <summary>
        /// Parses args and stores parsed values in declared fields.
        /// </summary>
        /// <param name="args">Arguments to parse</param>
        /// <exception cref="ParserCodeException">Thrown when the class doesn't conform to the parser requirements</exception>
        /// <exception cref="ParseException">Thrown when parsed arguments don't satisfy declared option fields:
        ///     Fields not inheriting from predefined classes (descendants of `OptionBase<T>` or `ArgumentBase<T>`)
        ///     Defining argument fields without overriding `GetArgumentOrder`</exception>
        public void Parse(string[] args)
		{
            bool CheckPlainParamCount(IArgument[] orderedArguments, int argsLength)
            {
                 int min = orderedArguments
                    .Select(a => a.ParameterAccept.MinParamAmount)
                    .Aggregate(0, (acc, val) => acc + val);

                int max = orderedArguments
                   .Select(a => a.ParameterAccept.MaxParamAmount)
                   .Aggregate(0, (acc, val) => acc + val);

                return argsLength >= min && argsLength <= max;
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

            IOption TryGetOption(string name) => options.Find(o => o.MatchingOptionName(name));

            int GetOptionParamCount(IOption option, string[] args, int argIdx)
			{
                int result = 0;
                while (result < option.ParameterAccept.MaxParamAmount)
				{
                    int idx = argIdx + result + 1;
                    if (idx == args.Length || args[idx] == Delimiter || TryGetOption(args[idx]) != null)
                    {
                        if (result < option.ParameterAccept.MinParamAmount) throw new ParseException("Too few parameters passed to option");
                        else break;
                    }
                    result++;
                }
                return result;
			}



            int argIdx = 0;
            // parse options
            while (argIdx < args.Length)
			{
                var currentArg = args[argIdx];

                if (!currentArg.StartsWith("-")) break;
                if (currentArg == Delimiter)
                {
                    argIdx++;
                    break;
                }

                var option = TryGetOption(currentArg);
                if (option == null) throw new ParseException("Unrecognized option");

                var paramCount = GetOptionParamCount(option, args, argIdx);
                option.CallParse(args[(argIdx + 1)..(argIdx + paramCount + 1)]);
                argIdx += paramCount + 1;
			}

            var orderedArguments = GetArgumentOrder();

            if (!CheckPlainParamCount(orderedArguments, args.Length - argIdx))
                throw new ParseException("arguments are incompatible");

            // parse plain args
            for (var plainArgIdx = 0; plainArgIdx < orderedArguments.Length; plainArgIdx++)
			{
                var remainingArgumentsCount = args.Length - argIdx;
                var currentArg = orderedArguments[plainArgIdx];
                var valCount = GetPlainParamCount(plainArgIdx, remainingArgumentsCount);

                string[] vals = args[argIdx..(argIdx + valCount)];

                currentArg.CallParse(vals);
                argIdx += valCount;
            }

            //check if isMandatory are parsed
            foreach (var o in options)
                if (o.IsMandatory && !o.IsSet)
                    throw new ParseException("mandatory option not set");
		}

        /// <summary>
        /// Automatically generates help message from declared fields (name, description).
        /// </summary>
        public string GenerateHelp()
        {
            StringBuilder result = new StringBuilder();
			var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

            string GetArgumentName(IArgument argument)
            {
                string result = argument.Name;

                if (argument.ParameterAccept.MaxParamAmount > 1)
                    result += "...";
                if (argument.ParameterAccept.MinParamAmount == 0)
                    result = $"[{result}]";

                return result;
            }

            void AppendUsageExampleLine()
            {
                result.Append(processName);
                if (options.Count > 0) result.Append(" [options]");
                foreach (var plainArg in GetArgumentOrder())
                    result.Append($" {GetArgumentName(plainArg)}");
                result.Append("\n\n");
            }


            AppendUsageExampleLine();

            result.Append("Options:\n");
			foreach (var opt in options)
			{
                result.Append($"{opt.GetHelp()}");
			}

            result.Append("Arguments:\n");
            foreach (var plainArg in GetArgumentOrder())
                result.Append(plainArg.GetHelp());

            return result.ToString();
        }
    }
}
