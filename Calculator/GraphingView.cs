using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;

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

		AxesDrawer drawer = new AxesDrawer(UIColor.Black);
		public override void Draw(CoreGraphics.CGRect rect)
		{
			base.Draw(rect);
			drawer.DrawAxesInRect(Bounds, Origin, Scale);
		}

}
}
