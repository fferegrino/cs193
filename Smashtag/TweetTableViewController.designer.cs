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
	[Register ("TweetTableViewController")]
	partial class TweetTableViewController
	{
		[Outlet]
		UIKit.UITextField searchTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (searchTextField != null) {
				searchTextField.Dispose ();
				searchTextField = null;
			}
		}
	}
}
