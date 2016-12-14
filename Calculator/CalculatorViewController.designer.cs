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
    [Register ("CalculatorViewController")]
    partial class CalculatorViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel display { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel recentOperations { get; set; }

        [Action ("PerformOperation:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PerformOperation (UIKit.UIButton sender);

        [Action ("PressDot:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PressDot (UIKit.UIButton sender);

        [Action ("RestoreMemory:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RestoreMemory (UIKit.UIButton sender);

        [Action ("SaveMemory:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SaveMemory (UIKit.UIButton sender);

        [Action ("TouchDigit:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TouchDigit (UIKit.UIButton sender);

        [Action ("Undo")]
        [GeneratedCode ("iOS Designer", "1.0")]
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