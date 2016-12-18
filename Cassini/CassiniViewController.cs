using Foundation;
using System;
using UIKit;
using System.Linq;

namespace Cassini
{
    public partial class CassiniViewController : UIViewController, IUISplitViewControllerDelegate
    {
		// this is just our normal "put constants in a struct" thing
		// but we call it Storyboard, because all the constants in it
		// are strings in our Storyboard

		struct StoryboardId
		{
			public const string ShowImageSegue = "Show Image";
		}

		public CassiniViewController (IntPtr handle) : base (handle) { }

		// prepare for segue is called
		// even if we invoke the segue from code using performSegue (see below)

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == StoryboardId.ShowImageSegue)
			{
				var ivc = segue.DestinationViewController.ContentViewController() as ImageViewController;
				if (ivc != null)
				{
					var imageName = (sender as UIButton)?.CurrentTitle;
					ivc.ImageURL = DemoURL.NASAImageNamed(imageName);
					ivc.Title = imageName;
				}
			}
		}

		// we changed the buttons to this target/action method
		// so that we could either
		// a) just set our imageURL in the detail if we're in a split view, or
		// b) cause the segue to happen from code with performSegue
		// to make the latter work, we had to create a segue in our storyboard
		// that was ctrl-dragged from the view controller icon (orange one at the top)

		partial void ShowImage(UIButton sender)
		{

			var ivc = SplitViewController.ViewControllers.Last().ContentViewController() as ImageViewController;
			if(ivc != null)
			{
				var imageName = sender.CurrentTitle;
				ivc.ImageURL = DemoURL.NASAImageNamed(imageName);
				ivc.Title = imageName;
			} else {
				PerformSegue(StoryboardId.ShowImageSegue, sender: sender);

			}
		}

		// if we are in a split view, we set ourselves as its delegate
		// this is so we can prevent an empty detail from collapsing on top of our master
		// see split view delegate method below

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			SplitViewController.Delegate = this;
		}

		// this method lets the split view's delegate
		// collapse the detail on top of the master when it's the detail's time to appear
		// this method returns whether we (the delegate) handled doing this
		// we don't want an empty detail to collapse on top of our master
		// so if the detail is an empty ImageViewController, we return true
		// (which tells the split view controller that we handled the collapse)
		// of course, we didn't actually handle it, we did nothing
		// but that's exactly what we want (i.e. no collapse if the detail ivc is empty)

		[Export("splitViewController:collapseSecondaryViewController:ontoPrimaryViewController:")]
		public bool CollapseSecondViewController(UISplitViewController splitViewController, UIViewController secondaryViewController, UIViewController primaryViewController)
		{
			if (primaryViewController.ContentViewController() == this)
			{
				var ivc = secondaryViewController.ContentViewController() as ImageViewController;
				if (ivc != null && ivc.ImageURL == null)
				{
					return true;
	
				}
			}
			return false;
		}

	}

	// a little helper extension
	// which either returns the view controller you send it to
	// or, if you send it to a UINavigationController,
	// it returns its visibleViewController
	// (if any, otherwise the UINavigationController itself)

	public static class UIViewControllerExtensions 
	{
		public static UIViewController ContentViewController(this UIViewController viewController)
		{
			var navcon = viewController as UINavigationController;
			return navcon?.VisibleViewController ?? viewController;
		}
	}
}