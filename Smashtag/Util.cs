using System;
using UIKit;

namespace Smashtag
{
	public static class ViewControllerExtensions
	{
		public static UIViewController ContentViewController(this UIViewController viewController)
		{
			var navcon = viewController as UINavigationController;
			return navcon?.VisibleViewController ?? viewController;
		}
	}
}
