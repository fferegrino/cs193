using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;
using static System.Math;

namespace Calculator
{
	[Register("GraphingView"), DesignTimeVisible(true)]
	public class GraphingView : UIView
	{
		public GraphingView(IntPtr p)
            : base(p)
        {
		}

		public GraphingView()
		{
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			_previous = Origin;
			Origin = new CGPoint(Bounds.GetMidX(), Bounds.GetMidY());
		}

		public void ChangeOrigin(UITapGestureRecognizer doubleTap)
		{
			System.Diagnostics.Debug.WriteLine($"Double tap!");
			Origin = doubleTap.LocationInView(this);
			//throw new NotImplementedException();
		}


		public void ChangeScale(UIPinchGestureRecognizer recognizer)
		{
			switch (recognizer.State)
			{
				case UIGestureRecognizerState.Changed:
				case UIGestureRecognizerState.Ended:
					Scale *= recognizer.Scale;
					recognizer.Scale = 1;
					break;
				default:
					break;
			}
		}

		CGPoint _previous;
		public void PanGraph(UIPanGestureRecognizer pan)
		{
			var point = pan.LocationInView(this);
			switch (pan.State)
			{
				case UIGestureRecognizerState.Changed:
					var difX = _previous.X - point.X;
					var difY = _previous.Y - point.Y;
					Origin = new CGPoint(Origin.X - difX, Origin.Y - difY);
					break;
				default:
					break;
			}
			_previous = point;
		}

		private double _scale = 10;
		public double Scale
		{
			get { return _scale; }
			set { _scale = value; SetNeedsDisplay(); }
		}

		private CGPoint _origin;
		public CGPoint Origin
		{
			get { return _origin; }
			set { _origin = value; SetNeedsDisplay(); }
		}

		Func<double, double> DrawingFunc;

		AxesDrawer drawer = new AxesDrawer(UIColor.Black);
		public override void Draw(CoreGraphics.CGRect rect)
		{
			base.Draw(rect);
			drawer.DrawAxesInRect(Bounds, Origin, Scale);

			var line = new UIBezierPath();
			line.LineWidth = (nfloat)2;
			UIColor.Magenta.SetStroke();
			UIColor.Magenta.SetFill();
			double starting = drawer.MinSpanX;
			double end = drawer.MaxSpanX;
			double x = starting;
			double step = 0.3;

			DrawingFunc = (t) => Sin(t);

			// Move line to start:
			line.MoveTo(MoveToOrigin(x, DrawingFunc(x)));

			while (x <= end)
			{
				var y = DrawingFunc(x);
				line.AddLineTo(MoveToOrigin(x, y));
				x += step;
			}
			line.Stroke();
			//line.Fill();

		}

		UIBezierPath DotAt(double x, double y)
		{
			return UIBezierPath.FromArc(MoveToOrigin(Align(x), Align(y)), 10, 0, new nfloat(2 * Math.PI), true);
		}


		void DrawControlPoints()
		{
			UIColor.Red.SetStroke();
			var path = UIBezierPath.FromArc(MoveToOrigin(0, 0), 10, 0, new nfloat(2 * Math.PI), true);
			path.Stroke();

			UIColor.Blue.SetStroke();
			path = UIBezierPath.FromArc(MoveToOrigin(-100, 100), 10, 0, new nfloat(2 * Math.PI), true);
			path.Stroke();

			UIColor.Brown.SetStroke();
			path = UIBezierPath.FromArc(MoveToOrigin(-100, -100), 10, 0, new nfloat(2 * Math.PI), true);
			path.Stroke();

			UIColor.Magenta.SetStroke();
			path = UIBezierPath.FromArc(MoveToOrigin(100, -100), 10, 0, new nfloat(2 * Math.PI), true);
			path.Stroke();

			UIColor.Purple.SetStroke();
			path = UIBezierPath.FromArc(MoveToOrigin(100, 100), 10, 0, new nfloat(2 * Math.PI), true);
			path.Stroke();
		}

		CGPoint MoveToOrigin(double x, double y)
		{
			return new CGPoint(Origin.X+x * Scale, Origin.Y-y *Scale);	
		}

		CGPoint MoveToOrigin(CGPoint original)
		{
			return new CGPoint(original.X + Origin.X, original.Y + Origin.Y);
		}

		double Align(double x)
		{
			return Round(x * Scale) / Scale;
		}
	}
}
