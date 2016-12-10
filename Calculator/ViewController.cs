using System;

using UIKit;

namespace Calculator
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		bool userIsInTheMiddleOfTyping;

		partial void TouchDigit(UIButton sender)
		{
			var digit = sender.CurrentTitle;
			if (userIsInTheMiddleOfTyping)
			{
				var textCurrentlyInDisplay = display.Text;
				display.Text = textCurrentlyInDisplay + digit;
			}
			else 
			{
				display.Text = digit;
			}
			brain.SetOperand(DisplayValue);
			userIsInTheMiddleOfTyping = true;
		}

		CalculatorBrain brain = new CalculatorBrain();

		Double DisplayValue
		{
			get { return Double.Parse(display.Text); }
			set { display.Text = value.ToString(); }
		}


		partial void PerformOperation(UIButton sender)
		{
			userIsInTheMiddleOfTyping = false;
			var symbol = sender.CurrentTitle;
			brain.PerformOperation(symbol);
			DisplayValue = brain.Result;
		}
	}
}
