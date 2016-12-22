// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

namespace Smashtag
{
	public partial class TweetImageViewCell : UITableViewCell
	{
		public TweetImageViewCell (IntPtr handle) : base (handle)
		{
		}


		public Uri ImageURL
		{
			get;
			set;
		}

		void FetchImage()
		{
			var url = ImageURL;
			if (url != null)
			{
				new System.Threading.Thread(new System.Threading.ThreadStart(() =>
				{
					var contentsOfUrl = NSData.FromUrl(url);

					InvokeOnMainThread(() =>
					{
						if (url == this.ImageURL)
						{
							var imageData = contentsOfUrl;
							if (imageData != null)
							{
								tweetImageView.Image = new UIImage(imageData);
								tweetImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
							}
							else
							{
							}
						}
					});
				})).Start();
			}
		}

		public void SetImage(string url)
		{

			if (!String.IsNullOrEmpty(url))
			{
				ImageURL = new Uri(url);
				FetchImage();
			}
		}
	}
}
