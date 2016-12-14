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


        [Action ("Undo")]
        partial void Undo ();

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