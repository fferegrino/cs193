// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Smashtag
{
	partial class TweetTableViewCell
	{
		[Outlet]
		UIKit.UILabel tweetCreatedLabel { get; set; }

		[Outlet]
		UIKit.UIImageView tweetProfileImageView { get; set; }

		[Outlet]
		UIKit.UILabel tweetScreenNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel tweetTextLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tweetScreenNameLabel != null) {
				tweetScreenNameLabel.Dispose ();
				tweetScreenNameLabel = null;
			}

			if (tweetTextLabel != null) {
				tweetTextLabel.Dispose ();
				tweetTextLabel = null;
			}

			if (tweetCreatedLabel != null) {
				tweetCreatedLabel.Dispose ();
				tweetCreatedLabel = null;
			}

			if (tweetProfileImageView != null) {
				tweetProfileImageView.Dispose ();
				tweetProfileImageView = null;
			}
		}
	}
}
