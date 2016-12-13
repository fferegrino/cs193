
using System;
using System.Linq;
using System.Collections.Generic;
using Foundation;

namespace Calculator
{
    public class CalculatorBrain
    {
        double _accumulator;
        const double IntegerPositions = 10;
        const int MaxMemory = 10;

		public CalculatorBrain()
		{
			VariableValues = new Dictionary<string, double>();
		}

		string lastVariable = null;
		internal void SetOperand(string variableName)
		{
			double variableValue;
			VariableValues.TryGetValue(variableName, out variableValue);
			_accumulator = variableValue;


			if(_pendingOperation == null)
			{
				_description = $"{variableName}";
			}

			lastVariable = variableName;
			_internalProgram.Add(variableName);
				
		}

		internal Dictionary<string, double> VariableValues { get; private set;}

		internal void SetOperand(double operand)
        {
			_accumulator = operand;
			if(_pendingOperation == null) 
			{
				_description = $"{_accumulator}";
			}
			lastVariable = null;
			_internalProgram.Add(operand);
        }

        enum Operation
        {
            Constant,
            UnaryOperation,
            BinaryOperation,
            Equals,
            //Dot,
            Clear
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
            { "C", Operation.Clear },
            { "√", Operation.UnaryOperation },
            { "±", Operation.UnaryOperation },
            { "cos", Operation.UnaryOperation  },
            { "sin", Operation.UnaryOperation },
            { "tan", Operation.UnaryOperation  },
			{ "^2", Operation.UnaryOperation },
			{ "^3", Operation.UnaryOperation },
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

        internal void PerformOperation(string symbol)
        {
            Operation op;
			_internalProgram.Add(symbol);
            if (operations.TryGetValue(symbol, out op))
            {
                AddOperationToDescription(symbol, op);
                switch (op)
                {
                    case Operation.Constant:
                        _accumulator = constants[symbol];
                        break;
                    case Operation.UnaryOperation:
						PerformPendingOperation();
                        _accumulator = unaries[symbol](_accumulator);
                        break;
                    case Operation.BinaryOperation:
                        PerformPendingOperation();
                        _pendingOperation = new PendingOperationInfo(binaries[symbol], _accumulator);
                        break;
                    case Operation.Equals:
                        PerformPendingOperation();
                        break;
                    case Operation.Clear:
                        Clear();
                        break;
                    default:
                        break;
                }
                _previousOperation = op;
            }
        }

        void Clear()
		{
			VariableValues.Remove("M");
			_internalProgram.Clear();
            _description = null;
            _accumulator = 0;
			_previousOperation = null;
            _pendingOperation = null;
        }

        private string _description;
        internal string Description => _description ?? " ";

        internal bool IsPartialResult => _pendingOperation != null;


        private Operation? _previousOperation;
		private void AddOperationToDescription(string symbol, Operation operation)
        {
            switch (operation)
            {
                case Operation.Constant:
                    if (IsPartialResult)
                        _description = $"{_description} {symbol}";
                    else
                        _description = $"{symbol}";
                    break;
                case Operation.UnaryOperation:
					if (lastVariable != null)
					{
						_description += " " + lastVariable;
						lastVariable = null;
					}
                    _description = $" {symbol} ({_description ?? _accumulator.ToString()})";
                    break;
                case Operation.BinaryOperation:
                    if (IsPartialResult) break;
                    _description = $"({_description ?? _accumulator.ToString()}) {symbol} ";
                    break;
                case Operation.Equals:
					if (_previousOperation.HasValue)
					{
						if (lastVariable != null)
						{
							_description = $"{_description} {lastVariable}";
							lastVariable = null;
						}
						else if(_previousOperation.Value != Operation.Constant &&
						_previousOperation.Value != Operation.Equals)
						{
							_description = $"{_description} {_accumulator}";
						}

					}
                    break;
                default:
                    break;
            }

        }


		private List<Object> _internalProgram  = new List<Object>();
		public Object Program { get { return new List<object>(_internalProgram); } set 
			{
				Clear();
				var program = value as List<Object>;
				if (program != null)
				{
					foreach (var item in program)
					{
						System.Diagnostics.Debug.WriteLine($"Prev {_previousOperation} Current {item}");
						var symbol = item as string;
						if (symbol != null && operations.ContainsKey(symbol))
						{
							PerformOperation(symbol);
							continue;
						}

						if(symbol != null)
						{
							SetOperand(symbol);
							continue;
						}

						try
						{
							var operand = (double)item;
							if (_previousOperation == null || _previousOperation == Operation.BinaryOperation)
								SetOperand(operand);
						}
						catch
						{
						}
					}
				}
			} 
		}

        internal Double Result
        {
            get { return _accumulator; }
        }
    }
}
