using System;
using Foundation;

namespace FaceIt
{
	[Register("BlinkingFaceViewController")]
	public class BlinkingFaceViewController : FaceViewController
	{
		public BlinkingFaceViewController(IntPtr handle) : base(handle)
		{

		}

		private bool _blinking;
		public bool Blinking
		{
			get { return _blinking; }
			set { _blinking = value; StartBlink(); }
		}

		private struct BlinkRate
		{
			public const double ClosedDuration = 0.4;
			public const double OpenDuration = 2.5;
		}

		void StartBlink(NSTimer obj = null)
		{
			if (Blinking)
			{
				FaceView.EyesOpen = false;
				NSTimer.CreateScheduledTimer(BlinkRate.ClosedDuration, EndBlink);
			}
		}

		void EndBlink(NSTimer obj)
		{
			if (Blinking)
			{
				FaceView.EyesOpen = true;
				NSTimer.CreateScheduledTimer(BlinkRate.OpenDuration, StartBlink);
			}
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			Blinking = true;
		}


		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			Blinking = false;
		}
	}
}
