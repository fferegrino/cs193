using System;
using System.ComponentModel;
using Foundation;
using UIKit;
using CoreGraphics;

namespace FaceIt
{
	[Register("FaceView"), DesignTimeVisible(true)]
	public class FaceView : UIView
	{
		private double _skullRadius;
		public double SkullRadius
		{
			get { return _skullRadius; }
			set { _skullRadius = value; }
		}

		public CGPoint SkullCenter
		{
			get; set;
		}

		public FaceView(IntPtr p)
            : base(p)
        {
			//Initialize();
		}

		public FaceView()
		{
			//Initialize();
		}

		public override void Draw(CoreGraphics.CGRect rect)
		{
			base.Draw(rect);

				_skullRadius = Math.Min(Bounds.Size.Width, Bounds.Size.Height);
				SkullCenter = new CGPoint(Bounds.GetMidX(), Bounds.GetMidY());

				var skull = UIBezierPath.FromArc(SkullCenter, new nfloat(_skullRadius), 0, new nfloat(2 * Math.PI), false);
				skull.LineWidth = (nfloat)5.0;

				UIColor.Blue.SetStroke();
				skull.Stroke();
		}
	}
}
