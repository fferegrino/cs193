using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace FaceIt
{
    public partial class FaceViewController : UIViewController
    {
        public FaceViewController (IntPtr handle) : base (handle)
        {
        }


        private FacialExpression _expression = new FacialExpression()
        {
            Eyes = Eyes.Open,
            EyeBrows = EyeBrows.Furrowed,
            Mouth = Mouth.Smile
        };

        public FacialExpression Expression
        {
            get { return _expression; }
            set { _expression = value; UpdateUI(); }
        }

        Dictionary<Mouth, double> mouthCurvatures = new Dictionary<Mouth, double>
        {
            { Mouth.Frown, -1 },
            { Mouth.Grin, 0.5 },
            { Mouth.Smile, 1 },
            { Mouth.Smirk, -0.5 },
            { Mouth.Neutral, 0 },
        };

        Dictionary<EyeBrows, double> eyeBrowsTilts = new Dictionary<EyeBrows, double>
        {
            { EyeBrows.Relaxed, 0.5 },
            { EyeBrows.Furrowed, -0.5 },
            { EyeBrows.Normal, 0 }
        };

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            UpdateUI();
        }

        void UpdateUI()
        {
            switch (Expression.Eyes)
            {
                case Eyes.Open:
                    FaceView.EyesOpen = true;
                    break;
                case Eyes.Closed:
                case Eyes.Squinting:
                    FaceView.EyesOpen = false;
                    break;
            }
            FaceView.MouthCurvature = mouthCurvatures[Expression.Mouth];
            FaceView.EyeBrowTilt = eyeBrowsTilts[Expression.EyeBrows];
        }
    }
}