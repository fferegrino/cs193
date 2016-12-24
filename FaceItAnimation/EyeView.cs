using System;
using UIKit;
using CoreGraphics;

namespace FaceIt
{
	public class EyeView : UIView
	{

		private float _lineWidth = 5f;
		public float LineWidth
		{
			get { return _lineWidth; }
			set { _lineWidth = value; SetNeedsDisplay(); }
		}

		private UIColor _color;
		public UIColor Color
		{
			get { return _color; }
			set { _color = value; SetNeedsDisplay(); }
		}

		private bool _eyesOpen;
		public bool EyesOpen
		{
			get { return _eyesOpen; }
			set { _eyesOpen = value; SetNeedsDisplay(); }
		}

		public override void Draw(CoreGraphics.CGRect rect)
		{
			UIBezierPath path;
			if (EyesOpen)
			{
				path = UIBezierPath.FromOval(Bounds.Inset(LineWidth / 2, LineWidth / 2));
			}
			else 
			{
				path = new UIBezierPath();
				path.MoveTo(new CGPoint(Bounds.GetMinX(), Bounds.GetMidY()));
				path.AddLineTo(new CGPoint(Bounds.GetMaxX(), Bounds.GetMidY()));
			}

			path.LineWidth = LineWidth;
			Color.SetStroke();
			path.Stroke();
		}

	}
}
