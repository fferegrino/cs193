// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Cassini
{
	[Register ("ImageViewController")]
	partial class ImageViewController
	{

		[Outlet]
		UIKit.UIActivityIndicatorView spinner { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (spinner != null) {
				spinner.Dispose ();
				spinner = null;
			}

			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}
		}
	}
}
