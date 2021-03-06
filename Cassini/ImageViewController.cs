// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

namespace Cassini
{
	public partial class ImageViewController : UIViewController, IUIScrollViewDelegate
	{
		public ImageViewController(IntPtr handle) : base(handle)
		{

		}

		Uri _imageURL;
		public Uri ImageURL
		{
			get
			{
				return _imageURL;
			}
			internal set
			{
				_imageURL = value;
				if (View.Window != null)
				{
					FetchImage();
				}
			}
		}

		void FetchImage()
		{
			var url = ImageURL;
			if (url != null)
			{
				// fire up the spinner
				// because we're about to fork something off on another thread
				spinner?.StartAnimating();
				// put a closure on the "user initiated" system queue
				// this closure does NSData(contentsOfURL:) which blocks
				// waiting for network response
				// it's fine for it to block the "user initiated" queue
				// because that's a concurrent queue
				// (so other closures on that queue can run concurrently even as this one's blocked)
				new System.Threading.Thread(new System.Threading.ThreadStart(() =>
				{
					var contentsOfUrl = NSData.FromUrl(url);
					// now that we got the data from the network
					// we want to put it up in the UI
					// but we can only do that on the main queue
					// so we queue up a closure here to do that

					InvokeOnMainThread(() =>
					{
						// since it could take a long time to fetch the image data
						// we make sure here that the image we fetched
						// is still the one this ImageViewController wants to display!
						// you must always think of these sorts of things
						// when programming asynchronously
						if (url == this.ImageURL)
						{
							var imageData = contentsOfUrl;
							if (imageData != null)
							{
								this.image = new UIImage(imageData);
							}
							else
							{
								this.spinner?.StopAnimating();
							}
						}

						else
						{
							// just so you can see in the console when this happens
							System.Diagnostics.Debug.WriteLine($"ignored data returned from url {url}");
						}
					});
				})).Start();
			}
		}

		UIKit.UIScrollView _scrollView;
		[Outlet]
		UIKit.UIScrollView scrollView 
		{ 
			get { return _scrollView; }
			set
			{
				_scrollView = value;
				_scrollView.ContentSize = _imageView.Frame.Size;
				// all three of the next lines of code
				// are necessary to make zooming work
				scrollView.Delegate = this;
				scrollView.MinimumZoomScale = (System.nfloat)0.03;
				scrollView.MaximumZoomScale = (System.nfloat)1.0;
			}
		}

		// zooming will not work if you don't implement
		// this UIScrollViewDelegate method

		[Export("viewForZoomingInScrollView:")]
		public UIView ViewForZoomingInScrollView(UIScrollView scrollView)
		{
			return _imageView;
		}

		UIKit.UIImageView _imageView = new UIImageView();

		//UIKit.UIImage image { get; set; }
		// a little helper var
		// it just makes sure things are kept in sync
		// whenever we change the image we're displaying
		// it's purely to make our code look prettier elsewhere in this class

		private UIImage image
		{
			get { return _imageView.Image; }
			set 
			{
				_imageView.Image = value;
				_imageView.SizeToFit();
				if(_scrollView != null)
					_scrollView.ContentSize = _imageView.Frame.Size;
				spinner?.StopAnimating();
			}
    	}

		// we know we're going to go on screen in this method
		// so we can no longer wait to fire off our (expensive) image fetch
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			if (image == null )
			{
				FetchImage();
			}
		}

		// note that we build some of our UI in the storyboard
		// by dragging a UIScrollView out into our scene
		// and we build some of it here by adding our UIImageView
		// as a subview of the UIScrollView
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			_scrollView.AddSubview(_imageView);
		}
	}
}
