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
			brain.AddOperand(Double.Parse(digit), userIsInTheMiddleOfTyping);
			DisplayValue = brain.Result;
			recentOperations.Text = brain.RecentOperations;
			userIsInTheMiddleOfTyping = true;
		}

		CalculatorBrain brain = new CalculatorBrain();

		Double DisplayValue
		{
			get { return Double.Parse(display.Text); }
			set { display.Text = value.ToString(); }
		}

		partial void PressDot(UIButton sender)
		{
			var symbol = sender.CurrentTitle;
			brain.PerformOperation(symbol);
			DisplayValue = brain.Result;
		}

		partial void PerformOperation(UIButton sender)
		{
			userIsInTheMiddleOfTyping = false;
			var symbol = sender.CurrentTitle;
			brain.PerformOperation(symbol);
			DisplayValue = brain.Result;
			recentOperations.Text = brain.RecentOperations;
		}
	}
}
