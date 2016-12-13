// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Calculator
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UILabel display { get; set; }

		[Outlet]
		UIKit.UILabel recentOperations { get; set; }

		[Action ("PerformOperation:")]
		partial void PerformOperation (UIKit.UIButton sender);

		[Action ("PressDot:")]
		partial void PressDot (UIKit.UIButton sender);

		[Action ("RestoreMemory:")]
		partial void RestoreMemory (UIKit.UIButton sender);

		[Action ("SaveMemory:")]
		partial void SaveMemory (UIKit.UIButton sender);

		[Action ("TouchDigit:")]
		partial void TouchDigit (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (display != null) {
				display.Dispose ();
				display = null;
			}

			if (recentOperations != null) {
				recentOperations.Dispose ();
				recentOperations = null;
			}
		}
	}
}
