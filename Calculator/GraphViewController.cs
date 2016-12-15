// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

namespace Calculator
{
	public partial class GraphViewController : UIViewController
	{

		GraphingView _graphingView;
		[Outlet]
		GraphingView GraphingView { get { return _graphingView; } set { _graphingView = value; WireGraphingView(); } }

		void WireGraphingView()
		{
			var pinch = new UIPinchGestureRecognizer();
			pinch.AddTarget(() => GraphingView.ChangeScale(pinch));
			GraphingView.AddGestureRecognizer(pinch);

			var doubleTap = new UITapGestureRecognizer();
			doubleTap.NumberOfTapsRequired = 2;
			doubleTap.AddTarget(() => GraphingView.ChangeOrigin(doubleTap));
			GraphingView.AddGestureRecognizer(doubleTap);


            var pan = new UIPanGestureRecognizer();
			pan.AddTarget(() => GraphingView.PanGraph(pan));
			GraphingView.AddGestureRecognizer(pan);
		}

		public GraphViewController (IntPtr handle) : base (handle)
		{
		}

		object _program;
		public object Program { get; set; }

		CalculatorBrain brain = new CalculatorBrain();

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			GraphingView.DrawingFunc = Function;
		}

		double Function(double x)
		{
			brain.VariableValues["M"] = x;
			brain.Program = Program;
			return brain.Result;
		}
	}
}