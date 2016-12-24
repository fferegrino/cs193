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
		#region Ctors
		public FaceView(IntPtr p)
			: base(p)
		{
			//Initialize();
		}

		public FaceView()
		{
			//Initialize();
		}
		#endregion

		public double _scale = 0.9;
		[Export(nameof(Scale)), Browsable(true)]
		public double Scale
		{
			get { return _scale; }
			set { _scale = value; SetNeedsDisplay(); }
		}

		double _mouthCurvature = 0.0;
		[Export(nameof(MouthCurvature)), Browsable(true)]
		public double MouthCurvature
		{
			get { return _mouthCurvature; }
			set { _mouthCurvature = value; SetNeedsDisplay(); }
		}

		bool _eyesOpen = true;
		[Export(nameof(EyesOpen)), Browsable(true)]
		public bool EyesOpen
		{
			get { return _eyesOpen; }
			set
			{
				_eyesOpen = value;
				LeftEye.EyesOpen = value;
				RightEye.EyesOpen = value;
			}
		}

		double _eyeBrowTilt = 0.6;
		[Export(nameof(EyeBrowTilt)), Browsable(true)]
		public double EyeBrowTilt
		{
			get { return _eyeBrowTilt; }
			set { _eyeBrowTilt = value; SetNeedsDisplay(); }
		}

		float _lineWidth = 5.0f;
		[Export(nameof(LineWidth)), Browsable(true)]
		public float LineWidth
		{
			get { return _lineWidth; }
			set
			{
				_lineWidth = value;
				SetNeedsDisplay();
				LeftEye.LineWidth = value;
				RightEye.LineWidth = value;
			}
		}

		UIColor _color = UIColor.Blue;
		[Export(nameof(Color)), Browsable(true)]
		public UIColor Color
		{
			get { return _color; }
			set
			{
				_color = value; SetNeedsDisplay();
				LeftEye.Color = value;
				RightEye.Color = value;
			}
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

		double SkullRadius => Math.Min(Bounds.Size.Width, Bounds.Size.Height) / 2 * Scale;


		CGPoint SkullCenter => new CGPoint(Bounds.GetMidX(), Bounds.GetMidY());

		struct Ratios
		{
			public const double SkullRadiusToEyeOffset = 3;
			public const double SkullRadiusToEyeRadius = 10;
			public const double SkullRadiusToMouthWidth = 1;
			public const double SkullRadiusToMouthHeight = 3;
			public const double SkullRadiusToMouthOffset = 3;
			public const double SkullRadiusToBrowOffset = 5;
		}

		enum Eye
		{
			Left,
			Right
		}

		UIBezierPath PathForCircleCenteredAtPoint(CGPoint center, double radius)
		{
			var path = UIBezierPath.FromArc(
				center,
				new nfloat(radius),
				0,
				new nfloat(2 * Math.PI),
				false);

			path.LineWidth = (nfloat)LineWidth;
			return path;
		}


		private CGPoint GetEyeCenter(Eye eye)
		{
			var eyeOffset = (nfloat)(SkullRadius / Ratios.SkullRadiusToEyeOffset);
			var eyeCenter = SkullCenter;
			eyeCenter.Y -= eyeOffset;
			switch (eye)
			{
				case Eye.Left:
					eyeCenter.X -= eyeOffset;
					break;
				case Eye.Right:
					eyeCenter.X += eyeOffset;
					break;
			}

			return eyeCenter;
		}

		EyeView _leftEye;
		EyeView LeftEye => _leftEye ?? (_leftEye = CreateEye());

		EyeView _rightEye;
		EyeView RightEye => _rightEye ?? (_rightEye = CreateEye());

		EyeView CreateEye()
		{
			var eye = new EyeView();
			eye.Opaque = false;
			eye.LineWidth = LineWidth;
			eye.Color = Color;
			AddSubview(eye);
			return eye;
		}

		void PositionEye(EyeView eye, CGPoint center)
		{
			var size = SkullRadius / Ratios.SkullRadiusToEyeRadius * 2;
			eye.Frame = new CGRect(CGPoint.Empty, new CGSize(size, size));
			eye.Center = center;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			PositionEye(LeftEye, GetEyeCenter(Eye.Left));
			PositionEye(RightEye, GetEyeCenter(Eye.Right));
		}

		//UIBezierPath PathForEye(Eye eye)
		//{
		//    var eyeRadius = SkullRadius / Ratios.SkullRadiusToEyeRadius;
		//    var eyeCenter = GetEyeCenter(eye);
		//    if (EyesOpen)
		//    {
		//        return PathForCircleCenteredAtPoint(eyeCenter, eyeRadius);
		//    }
		//    else
		//    {
		//        var path = new UIBezierPath();
		//        path.MoveTo(new CGPoint(eyeCenter.X - eyeRadius, eyeCenter.Y));
		//        path.AddLineTo(new CGPoint(eyeCenter.X + eyeRadius, eyeCenter.Y));
		//        path.LineWidth = (nfloat)LineWidth;
		//        return path;
		//    }
		//}

		UIBezierPath PathForBrow(Eye eye)
		{
			var tilt = EyeBrowTilt;
			if (eye == Eye.Left) tilt *= -1;

			var browCenter = GetEyeCenter(eye);
			browCenter.Y -= (nfloat)(SkullRadius / Ratios.SkullRadiusToBrowOffset);
			var eyeRadius = SkullRadius / Ratios.SkullRadiusToEyeRadius;
			var tiltOffset = Math.Max(-1, Math.Min(tilt, 1)) * eyeRadius / 2;
			var browStart = new CGPoint(browCenter.X - eyeRadius, browCenter.Y - tiltOffset);
			var browEnd = new CGPoint(browCenter.X + eyeRadius, browCenter.Y + tiltOffset);

			var path = new UIBezierPath();
			path.MoveTo(browStart);
			path.AddLineTo(browEnd);
			path.LineWidth = (nfloat)LineWidth;

			return path;
		}

		UIBezierPath PathForMouth()
		{
			var mouthWidth = SkullRadius / Ratios.SkullRadiusToMouthWidth;
			var mouthHeight = SkullRadius / Ratios.SkullRadiusToMouthHeight;
			var mouthOffset = SkullRadius / Ratios.SkullRadiusToMouthOffset;

			var mouthRect = new CGRect(SkullCenter.X - mouthWidth / 2, SkullCenter.Y + mouthOffset, mouthWidth, mouthHeight);

			var smileOffset = Math.Max(-1, Math.Min(MouthCurvature, 1)) * mouthHeight;
			var start = new CGPoint(mouthRect.GetMinX(), mouthRect.GetMinY());
			var end = new CGPoint(mouthRect.GetMaxX(), mouthRect.GetMinY());
			var cp1 = new CGPoint(mouthRect.GetMinX() + mouthRect.Width / 3, mouthRect.GetMinY() + smileOffset);
			var cp2 = new CGPoint(mouthRect.GetMaxX() - mouthRect.Width / 3, mouthRect.GetMinY() + smileOffset);

			var path = new UIBezierPath();
			path.MoveTo(start);
			path.AddCurveToPoint(end, cp1, cp2);
			path.LineWidth = (nfloat)LineWidth;

			return path;
		}

		public override void Draw(CoreGraphics.CGRect rect)
		{
			Color.SetStroke();
			PathForCircleCenteredAtPoint(SkullCenter, SkullRadius).Stroke();
			//PathForEye(Eye.Left).Stroke();
			//PathForEye(Eye.Right).Stroke();
			PathForBrow(Eye.Left).Stroke();
			PathForBrow(Eye.Right).Stroke();
			PathForMouth().Stroke();
		}
	}
}
