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
        double Scale { get; set; } = 0.9;

        double MouthCurvature = 0.0;

        double SkullRadius => Math.Min(Bounds.Size.Width, Bounds.Size.Height) / 2 * Scale;

        CGPoint SkullCenter => new CGPoint(Bounds.GetMidX(), Bounds.GetMidY());

        struct Ratios
        {
            public const double SkullRadiusToEyeOffset = 3;
            public const double SkullRadiusToEyeRadius = 10;
            public const double SkullRadiusToMouthWidth = 1;
            public const double SkullRadiusToMouthHeight = 3;
            public const double SkullRadiusToMouthOffset = 3;
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

            path.LineWidth = (nfloat)5.0;
            return path;
        }


        private CGPoint GetEyeCenter(Eye eye)
        {
            var eyeOffset = (nfloat)(SkullRadius / Ratios.SkullRadiusToEyeOffset);
            var eyeCenter = SkullCenter;
            eyeCenter.Y -= eyeOffset;
            switch(eye)
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

        UIBezierPath PathForEye(Eye eye)
        {
            var eyeRadius = SkullRadius / Ratios.SkullRadiusToEyeRadius;
            var eyeCenter = GetEyeCenter(eye);
            return PathForCircleCenteredAtPoint(eyeCenter, eyeRadius);
        }

        UIBezierPath PathForMouth()
        {
            var mouthWidth = SkullRadius / Ratios.SkullRadiusToMouthWidth;
            var mouthHeight = SkullRadius / Ratios.SkullRadiusToMouthHeight;
            var mouthOffset= SkullRadius / Ratios.SkullRadiusToMouthOffset;

            var mouthRect = new CGRect(SkullCenter.X - mouthWidth / 2, SkullCenter.Y + mouthOffset, mouthWidth, mouthHeight);

            var smileOffset = Math.Max(-1, Math.Min(MouthCurvature, 1)) * mouthHeight;
            var start = new CGPoint(mouthRect.GetMinX(), mouthRect.GetMinY());
            var end = new CGPoint(mouthRect.GetMaxX(), mouthRect.GetMinY());
            var cp1 = new CGPoint(mouthRect.GetMinX() + mouthRect.Width / 3, mouthRect.GetMinY() + smileOffset);
            var cp2 = new CGPoint(mouthRect.GetMaxX() - mouthRect.Width / 3, mouthRect.GetMinY() + smileOffset);

            var path = new UIBezierPath();
            path.MoveTo(start);
            path.AddCurveToPoint(end, cp1, cp2);
            path.LineWidth = (nfloat)5.0;

            return path;
        }

        public override void Draw(CoreGraphics.CGRect rect)
        {
            UIColor.Blue.SetStroke();
            PathForCircleCenteredAtPoint(SkullCenter, SkullRadius).Stroke();
            PathForEye(Eye.Left).Stroke();
            PathForEye(Eye.Right).Stroke();
            PathForMouth().Stroke();
        }
    }
}
