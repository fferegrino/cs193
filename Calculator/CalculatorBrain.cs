
using System;
using System.Linq;
using System.Collections.Generic;

namespace Calculator
{
    public class CalculatorBrain
    {
        double _accumulator;
        bool _hasSetDot;
        double DecimalPositions = 10;
        const double IntegerPositions = 10;
        const int MaxMemory = 10;


        internal void AddOperand(double operand, bool userIsInTheMiddleOfTyping)
        {
            if (userIsInTheMiddleOfTyping)
            {
                if (!_hasSetDot)
                {
                    _accumulator = _accumulator * IntegerPositions + operand;
                }
                else
                {
                    _accumulator = _accumulator + operand / DecimalPositions;
                    DecimalPositions *= 10;
                }
            }
            else
            {
                DecimalPositions = 10;
                if (!_hasSetDot)
                {
                    _accumulator = operand;
                    //_integerPositions *= 10;
                }
                else
                {
                    _accumulator = operand / DecimalPositions;
                    DecimalPositions *= 10;
                }
            }
        }

        enum Operation
        {
            Constant,
            UnaryOperation,
            BinaryOperation,
            Equals,
            Dot,
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
            { "×", Operation.BinaryOperation },
            { "÷", Operation.BinaryOperation },
            { "−", Operation.BinaryOperation },
            { "+", Operation.BinaryOperation },
            { "=", Operation.Equals },
            { ".", Operation.Dot }
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
            { "^2", (d) => d * d }
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
            if (operations.TryGetValue(symbol, out op))
            {
                AddRecentOp(symbol, op);
                switch (op)
                {
                    case Operation.Constant:
                        _accumulator = constants[symbol];
                        break;
                    case Operation.UnaryOperation:
                        _accumulator = unaries[symbol](_accumulator);
                        break;
                    case Operation.BinaryOperation:
                        _hasSetDot = false;
                        PerformPendingOperation();
                        _pendingOperation = new PendingOperationInfo(binaries[symbol], _accumulator);
                        break;
                    case Operation.Equals:
                        PerformPendingOperation();
                        break;
                    case Operation.Dot:
                        if (!_hasSetDot)
                        {
                            _hasSetDot = true;
                        }
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
            _description = null;
            _hasSetDot = false;
            _accumulator = 0;
            _pendingOperation = null;
            DecimalPositions = 10;
        }

        private string _description;
        internal string Description => _description ?? " ";

        internal bool IsPartialResult => _pendingOperation != null;


        private Operation? _previousOperation;
        private void AddRecentOp(string symbol, Operation operation)
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
                    _description = $" {symbol} ({_description ?? _accumulator.ToString()})";
                    break;
                case Operation.BinaryOperation:
                    if (IsPartialResult) break;
                    _description = $"({_description ?? _accumulator.ToString()}) {symbol} ";
                    break;
                case Operation.Equals:
                    if (_previousOperation.HasValue && 
                        _previousOperation.Value != Operation.Constant &&
                        _previousOperation.Value != Operation.Equals)
                    {
                        if (_description != null)
                            _description = $"{_description} {_accumulator}";
                    }
                    break;
                default:
                    break;
            }
        }

        internal Double Result
        {
            get { return _accumulator; }
        }
    }
}
