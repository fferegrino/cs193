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
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        FaceIt.FaceView FaceView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (FaceView != null) {
                FaceView.Dispose ();
                FaceView = null;
            }
        }
    }
}