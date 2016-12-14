
using System;
using System.Linq;
using System.Collections.Generic;
using Foundation;

namespace Calculator
{
    public partial class CalculatorBrain
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
			_internalProgram.Push(variableName);
				
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
			_internalProgram.Push(operand);
        }

        enum Operation
        {
            Constant,
			UnaryOperation,
			UnaryOperationPostFix,
            BinaryOperation,
            Equals,
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
			{ "^2", Operation.UnaryOperationPostFix },
			{ "^3", Operation.UnaryOperationPostFix },
            { "×", Operation.BinaryOperation },
            { "÷", Operation.BinaryOperation },
            { "−", Operation.BinaryOperation },
            { "+", Operation.BinaryOperation },
            { "=", Operation.Equals }
        };

        internal void PerformOperation(string symbol)
        {
            Operation op;
			_internalProgram.Push(symbol);
            if (operations.TryGetValue(symbol, out op))
            {
                AddOperationToDescription(symbol, op);
                switch (op)
                {
                    case Operation.Constant:
                        _accumulator = constants[symbol];
                        break;
					case Operation.UnaryOperation:
					case Operation.UnaryOperationPostFix:
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
			//VariableValues.Remove("M");
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
				case Operation.UnaryOperationPostFix:
					if (lastVariable != null)
					{
						_description += " " + lastVariable;
						lastVariable = null;
					}
					if(operation== Operation.UnaryOperation)
                    	_description = $" {symbol}({_description ?? _accumulator.ToString()})";
					else
						_description = $"({_description ?? _accumulator.ToString()}){symbol}";
						
                    break;
                case Operation.BinaryOperation:
                    if (IsPartialResult) break;
                    _description = $"{_description ?? _accumulator.ToString()} {symbol} ";
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


		internal void UndoLastOperation()
		{
			if (_internalProgram.Count != 0)
				_internalProgram.Pop();//throw new NotImplementedException();
			else
				System.Diagnostics.Debug.WriteLine("No hay más operaciones");
		}

		public void ReRunProgram()
		{
			var newProgram = Program;
			Program = newProgram;
		}

		private Stack<Object> _internalProgram  = new Stack<Object>();
		public Object Program { get { return new Stack<object>(_internalProgram); } set 
			{
				Clear();
				var program = value as Stack<Object>;
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

						if (symbol != null)
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
