
using System;
using System.Collections.Generic;

namespace Calculator
{
	public class CalculatorBrain
	{
		double _accumulator;

		internal void SetOperand(double operand)
		{
			_accumulator = operand;
		}

		enum Operation
		{
			Constant,
			UnaryOperation,
			BinaryOperation,
			Equals
		}

		struct PendingOperationInfo
		{ 
			public PendingOperationInfo(Func<double, double, double> binaryOperation, double firstOperand)
			{
				BinaryOperation = binaryOperation;
				FirstOperand = firstOperand;
			}
			public double FirstOperand { get; private set; }
			public Func<double, double, double> BinaryOperation { get; private set; }

		}

		PendingOperationInfo? _pendingOperation;

		void PerformPendingOperation()
		{
			if (_pendingOperation.HasValue)
			{
				var p = _pendingOperation.Value;
				_accumulator = p.BinaryOperation(p.FirstOperand, _accumulator);
				_pendingOperation = null;
			}
		}

		Dictionary<string, Operation> operations = new Dictionary<string, Operation>
		{
			{ "π", Operation.Constant },
			{ "e", Operation.Constant },
			{ "√", Operation.UnaryOperation },
			{ "±", Operation.UnaryOperation },
			{ "×", Operation.BinaryOperation },
			{ "÷", Operation.BinaryOperation },
			{ "−", Operation.BinaryOperation },
			{ "+", Operation.BinaryOperation },
			{ "=", Operation.Equals }
		};

		Dictionary<string, double> constants = new Dictionary<string, double>
		{
			{ "π", Math.PI },
			{ "e", Math.E }
		};

		Dictionary<string, Func<double, double>> unaries = new Dictionary<string, Func<double, double>>
		{
			{ "√", Math.Sqrt },
			{ "±", (d ) => -1 * d },
		};

		Dictionary<string, Func<double,double,double>> binaries = new Dictionary<string, Func<double, double, double>>
		{
			{ "×", (a,b) => a * b },
			{ "÷", (a,b) => a / b },
			{ "−", (a,b) => a - b },
			{ "+", (a,b) => a + b },
		};

		internal void PerformOperation(string symbol)
		{
			Operation op;
			if (operations.TryGetValue(symbol, out op))
			{
				switch (op)
				{
					case Operation.Constant:
						_accumulator = constants[symbol];
						break;
					case Operation.UnaryOperation:
						_accumulator = unaries[symbol](_accumulator);
						break;
					case Operation.BinaryOperation:
						PerformPendingOperation();
						_pendingOperation = new PendingOperationInfo(binaries[symbol], _accumulator);
						break;
					case Operation.Equals:
						PerformPendingOperation();
						break;
					default:
						break;
				}
			}
		}

		internal Double Result
		{
			get { return _accumulator; }
		}
	}
}
