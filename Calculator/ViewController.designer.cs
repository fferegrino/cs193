// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Calculator
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel display { get; set; }

        [Action ("PerformOperation:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PerformOperation (UIKit.UIButton sender);

        [Action ("TouchDigit:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TouchDigit (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (display != null) {
                display.Dispose ();
                display = null;
            }
        }
    }
}