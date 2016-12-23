using System;
using System.Collections.Generic;
using UIKit;

namespace FaceIt
{
	public partial class EmotionsViewController : UIViewController
	{
		public EmotionsViewController() : base("EmotionsViewController", null)
		{
		}

		public EmotionsViewController(IntPtr handle) : base(handle)
        {
		}


		Dictionary<string, FacialExpression> emotionalFaces = new Dictionary<string, FacialExpression>()
		{
			{"angry", new FacialExpression { Eyes = Eyes.Closed, EyeBrows = EyeBrows.Furrowed, Mouth = Mouth.Frown } },
			{"happy", new FacialExpression { Eyes = Eyes.Open, EyeBrows = EyeBrows.Normal, Mouth = Mouth.Smile } },
			{"worried", new FacialExpression { Eyes = Eyes.Open, EyeBrows = EyeBrows.Relaxed, Mouth = Mouth.Smirk } },
			{"mischievious", new FacialExpression { Eyes = Eyes.Open, EyeBrows = EyeBrows.Furrowed, Mouth = Mouth.Grin } }
		};

		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			var destinationvc = segue.DestinationViewController;
			var navcon = destinationvc as UINavigationController;
			if (navcon != null)
			{
				destinationvc = navcon.VisibleViewController ?? destinationvc;
			}
			var facevc = destinationvc as FaceViewController;
			if (facevc != null && !String.IsNullOrEmpty(segue.Identifier))
			{
				FacialExpression expression;
				if (emotionalFaces.TryGetValue(segue.Identifier, out expression))
				{
					facevc.Expression = expression;
					var sendingButton = sender as UIButton;
					if (sendingButton != null)
						facevc.NavigationItem.Title = sendingButton.CurrentTitle;
				}
			}
		}
	}
}

