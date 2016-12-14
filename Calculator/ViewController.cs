using System;
using System.Globalization;
using UIKit;

namespace Calculator
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			display.AccessibilityIdentifier = "display";
		}


		bool userIsInTheMiddleOfTyping;
		static readonly string Dot = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;


		partial void Undo(){

			if (userIsInTheMiddleOfTyping)
			{
				var l = display.Text.Length;
				if (l == 1)
				{
					brain.UndoLastOperation();
					display.Text = "0";
					userIsInTheMiddleOfTyping = false;
				}
				else if (l > 1)
				{
					display.Text = display.Text.Substring(0, l - 1);
				}
				brain.SetOperand(DisplayValue);
			}
			else 
			{
				brain.UndoLastOperation();
				brain.ReRunProgram();
			}
			SetResult();
		}

		partial void RestoreMemory(UIKit.UIButton sender)
		{
			brain.SetOperand("M");
			DisplayValue = brain.Result;
		}

		partial void SaveMemory(UIKit.UIButton sender)
		{
			var program = brain.Program;
			brain.VariableValues["M"] = DisplayValue;
			brain.Program = program;
			SetResult();
		}

		partial void TouchDigit(UIButton sender)
		{
			 
			var digit = sender.CurrentTitle;
			if (userIsInTheMiddleOfTyping)
	      {
				var textCurrentlyInDisplay = display.Text;
				if (_hasDot && !DisplayHasDot)
				{
					display.Text = textCurrentlyInDisplay +  Dot + digit;
					_hasSetDot = true;
				}
				else 
				{

					display.Text = textCurrentlyInDisplay + digit;
				}
			}
			else 	
			{
				if (_hasDot && !DisplayHasDot)
				{
					display.Text = $"0{Dot}{digit}";
					_hasSetDot = true;
				}
				else
				{
					display.Text = digit;
				}
			}        
			brain.SetOperand(DisplayValue);
			userIsInTheMiddleOfTyping = true;
            SetResult();
		}

		CalculatorBrain brain = new CalculatorBrain();

		Double DisplayValue
		{
			get { return Double.Parse(display.Text); }
			set { display.Text = value.ToString(); }
		}

		bool DisplayHasDot => _hasSetDot;

        void SetResult()
        {
            DisplayValue = brain.Result;
			if(!System.String.IsNullOrEmpty( brain.Description))
            	recentOperations.Text = brain.Description +  (brain.IsPartialResult ? "..." : " =");
        }
		bool _hasDot;
		bool _hasSetDot;
		partial void PressDot(UIButton sender)
		{
			if (!_hasDot)
			{
				_hasDot = true;
			}

        }

		partial void PerformOperation(UIButton sender)
		{
			userIsInTheMiddleOfTyping = false;
			var symbol = sender.CurrentTitle;
			switch (symbol)
			{
				case "C":
				case "=":
					Clear();
					break;
					
			}
			brain.PerformOperation(symbol);
			if (brain.IsPartialResult)
				Clear();
            SetResult();
        }

		private void Clear()
		{
			_hasSetDot = false;
			_hasDot = false;
		}
	}
}
