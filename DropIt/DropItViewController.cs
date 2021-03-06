// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

namespace DropIt
{
	public partial class DropItViewController : UIViewController
	{
		public DropItViewController (IntPtr handle) : base (handle)
		{
		}

		DropIt.DropItView _gameView;
		[Outlet]
		DropIt.DropItView gameView
		{
			get
			{
				return _gameView;
			}
			set
			{
				_gameView = value;

				UITapGestureRecognizer tgr = new UITapGestureRecognizer(AddDrop);
				_gameView.AddGestureRecognizer(tgr);

				UIPanGestureRecognizer pgr =
					new UIPanGestureRecognizer(gameView.GrabDrop);
				gameView.AddGestureRecognizer(pgr);

				_gameView.RealGravity = true;

			}
		}

		void AddDrop(UITapGestureRecognizer recognizer)
		{
			if(recognizer.State == UIGestureRecognizerState.Ended)
			_gameView.AddDrop();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			_gameView.Animating = true;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			_gameView.Animating = false;
		}
	}
}
