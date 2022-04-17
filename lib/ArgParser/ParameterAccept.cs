using System;

namespace ArgParser
{
	/// <summary>
	/// Describes number of accepted parameters. The default is to accept exactly 1 parameter
	/// (the default parameterless constructor will construct such an instance).
	/// </summary>
	public struct ParameterAccept
	{
		private int minParamAmount;
		public int MinParamAmount
		{
			get
			{
				if (minParamAmount < 1 && maxParamAmount < 1) { return minParamAmount + 1; }
				return minParamAmount;
			}
		}

		private int maxParamAmount;
		public int MaxParamAmount
		{
			get
			{
				if (minParamAmount < 1 && maxParamAmount < 1) { return maxParamAmount + 1; }
				return maxParamAmount;
			}
		}

		public readonly static ParameterAccept Mandatory = new();
		public readonly static ParameterAccept Optional = new(0, 1);
		public readonly static ParameterAccept AtLeastOne = new(1, int.MaxValue);
		public readonly static ParameterAccept Any = new(0, int.MaxValue);
		internal static ParameterAccept Zero = new() { minParamAmount = -1, maxParamAmount = -1 };

		public bool IsVariadic => MinParamAmount != MaxParamAmount;


		public ParameterAccept(int paramAmount) : this(paramAmount, paramAmount) { }


		/// <exception cref="ArgumentException">Thrown when minParamAmount < 0 or maxParamAmount < minParamAmount or maxParamAmount == 0</exception>
		public ParameterAccept(int minParamAmount, int maxParamAmount)
		{
			if (minParamAmount < 0 || maxParamAmount < minParamAmount || maxParamAmount == 0)
				throw new ArgumentException("Wrong param amount.");

			this.minParamAmount = minParamAmount;
			this.maxParamAmount = maxParamAmount;
		}


		internal string GetHelp()
		{
			static bool EqualTo(ParameterAccept left, ParameterAccept right) =>
				left.MinParamAmount == right.MinParamAmount &&
				left.MaxParamAmount == right.MaxParamAmount;

			if (EqualTo(this, Mandatory) || EqualTo(this, Zero)) return "";
			if (!IsVariadic) return $"[{MinParamAmount}]";
			if (MaxParamAmount == int.MaxValue) return $"[{MinParamAmount}..]";
			return $"[{MinParamAmount}..{MaxParamAmount}]";
		}
	};
}