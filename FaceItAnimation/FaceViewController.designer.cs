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

namespace FaceIt
{
    [Register ("FaceViewController")]
    partial class FaceViewController
    {
        [Action ("ToggleEyes:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ToggleEyes (UIKit.UITapGestureRecognizer recognizer);

        void ReleaseDesignerOutlets ()
        {
            if (FaceView != null) {
                FaceView.Dispose ();
                FaceView = null;
            }
        }
    }
}