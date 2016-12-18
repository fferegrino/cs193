using System;
using Foundation;
using LinqToTwitter;
using UIKit;

namespace Smashtag
{
	[Register("TweetTableViewCell")]
	public partial class TweetTableViewCell : UITableViewCell
	{

		public TweetTableViewCell(IntPtr handle) : base(handle) { }

		private Status _tweet;
		public Status Tweet
		{
			get { return _tweet; }
			set { _tweet = value; UpdateUI(); }
		}

		void UpdateUI()
		{
			tweetCreatedLabel.Text = Tweet.CreatedAt.ToShortTimeString();
			tweetScreenNameLabel.Text = Tweet.User.ScreenNameResponse;
			tweetTextLabel.Text = Tweet.Text;

			if (!String.IsNullOrEmpty(Tweet.User.ProfileImageUrl))
			{
				var imageData = NSData.FromUrl(new Uri(Tweet.User.ProfileImageUrl));
				tweetProfileImageView.Image = new UIImage(imageData);
			}
		}

	}
}
