using System;
using System.Collections.Generic;

namespace Calculator
{
	public partial class CalculatorBrain
	{

		Dictionary<string, double> constants = new Dictionary<string, double>
		{
			{ "π", Math.PI },
			{ "e", Math.E }
		};

		Dictionary<string, Func<double, double>> unaries = new Dictionary<string, Func<double, double>>
		{
			{ "√", Math.Sqrt },
			{ "±", (d ) => -1 * d },
			{ "cos", Math.Cos },
			{ "sin", Math.Sin},
			{ "tan", Math.Tan },
			{ "^2", (d) => d * d },
			{ "^3", (d) => d * d }
		};

		Dictionary<string, Func<double, double, double>> binaries = new Dictionary<string, Func<double, double, double>>
		{
			{ "×", (a,b) => a * b },
			{ "÷", (a,b) => a / b },
			{ "−", (a,b) => a - b },
			{ "+", (a,b) => a + b },
		};
}
}
