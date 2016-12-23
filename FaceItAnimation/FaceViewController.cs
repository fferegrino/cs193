using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace FaceIt
{
    public partial class FaceViewController : UIViewController
    {
        public FaceViewController(IntPtr handle) : base(handle)
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

        FaceView _faceView;
        [Outlet]
        protected FaceView FaceView
        {
            get { return _faceView; }
            set
            {
                _faceView = value;
                // didSet:
                var gesture = new UIPinchGestureRecognizer();
                gesture.AddTarget(() => _faceView.ChangeScale(gesture));
                _faceView.AddGestureRecognizer(gesture);

                var happierSwipeGestureRecognizer = new UISwipeGestureRecognizer();
                happierSwipeGestureRecognizer.AddTarget(() => IncreaseHappiness());
                happierSwipeGestureRecognizer.Direction = UISwipeGestureRecognizerDirection.Up;
                _faceView.AddGestureRecognizer(happierSwipeGestureRecognizer);

                var sadderSwipeGestureRecognizer = new UISwipeGestureRecognizer();
                sadderSwipeGestureRecognizer.AddTarget(() => DecreaseHappiness());
                sadderSwipeGestureRecognizer.Direction = UISwipeGestureRecognizerDirection.Down;
                _faceView.AddGestureRecognizer(sadderSwipeGestureRecognizer);

				var rotationGestureRecognizer = new UIRotationGestureRecognizer();
				rotationGestureRecognizer.AddTarget(() => ChangeBrows(rotationGestureRecognizer));
				//rotationGestureRecognizer.AddTarget(() => ChangeBrows(gesture));
				_faceView.AddGestureRecognizer(rotationGestureRecognizer);

                UpdateUI();
            }
        }

		partial void ToggleEyes(UITapGestureRecognizer recognizer)
		{
			if (recognizer.State == UIGestureRecognizerState.Ended)
			{
				var newEyes = Expression.Eyes;
				switch (Expression.Eyes)
				{
					case Eyes.Closed:
						newEyes = Eyes.Open;
						break;
					case Eyes.Open:
						newEyes = Eyes.Closed;
						break;
					default:
						break;
				}

            	Expression = new FacialExpression
				{
					Mouth = Expression.Mouth,
					EyeBrows = Expression.EyeBrows,
					Eyes = newEyes
				};
			}
		}

		public void ChangeBrows(UIRotationGestureRecognizer recognizer)
		{
			var newEyeBrows = Expression.EyeBrows;

			switch (recognizer.State)
			{
				case UIGestureRecognizerState.Changed:
				case UIGestureRecognizerState.Ended:
					if (recognizer.Rotation > Math.PI / 6)
					{
						newEyeBrows = newEyeBrows.MoreRelaxedBrow();
						recognizer.Rotation = 0;
					}
					else if (recognizer.Rotation < -1 * Math.PI / 6)
					{
						newEyeBrows = newEyeBrows.MoreFurrowedBrow();
						recognizer.Rotation = 0;
					}
					break;
				default:
					break;
			}

			Expression = new FacialExpression
			{
				Mouth = Expression.Mouth,
				EyeBrows = newEyeBrows,
				Eyes = Expression.Eyes
			};
		}

        private void IncreaseHappiness()
        {
            Expression = new FacialExpression
            {
                Mouth = Expression.Mouth.HappierMouth(),
                EyeBrows = Expression.EyeBrows,
                Eyes = Expression.Eyes
            };
        }

        private void DecreaseHappiness()
        {
            Expression = new FacialExpression
            {
                Mouth = Expression.Mouth.SadderMouth(),
                EyeBrows = Expression.EyeBrows,
                Eyes = Expression.Eyes
            };
        }

        void UpdateUI()
        {
			if (FaceView != null)
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
}