using System;
using System.Linq;
using System.Collections.Generic;
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
			tweetTextLabel.AttributedText = GetAttributedText( Tweet);

			if (!String.IsNullOrEmpty(Tweet.User.ProfileImageUrl))
			{
				var imageData = NSData.FromUrl(new Uri(Tweet.User.ProfileImageUrl));
				tweetProfileImageView.Image = new UIImage(imageData);
			}
		}

		private NSAttributedString GetAttributedText(Status tweet)
		{
			var text = tweet.Text;
			var myAttribute = UIFont.FromName("Chalkduster", (System.nfloat)18.0);
			var myString = new NSMutableAttributedString(text, myAttribute);

			var hashtagLocations = tweet.Hashtags();
			for (int i = 0; i < hashtagLocations.Count; i+= 2)
			{
				var range = new NSRange(hashtagLocations[i], hashtagLocations[i+1]);
				myString.AddAttribute(UIStringAttributeKey.ForegroundColor, 
				                      UIColor.Blue, 
				                      range);
			}

			var mentions = tweet.Mentions();
			for (int i = 0; i < mentions.Count; i += 2)
			{
				var range = new NSRange(mentions[i], mentions[i + 1]);
				myString.AddAttribute(UIStringAttributeKey.ForegroundColor,
									  UIColor.Red,
									  range);
			}

			var links = tweet.Links();
			for (int i = 0; i < links.Count; i += 2)
			{
				var range = new NSRange(links[i], links[i + 1]);
				myString.AddAttribute(UIStringAttributeKey.ForegroundColor,
				                      UIColor.Green,
									  range);
			}

			return myString;
		}

	}


	struct Symbols
	{
		public const char H = '#';
		public const char M = 'M';
		//const string 
		public const char S = ' ';
	}

	static class TweetExtensions
	{

		public static List<int> Links(this Status tweet)
		{
			var r = tweet.Entities.
			             UrlEntities.
			             SelectMany((UrlEntity arg) =>
			 new int[] { arg.Start, arg.End - arg.Start });
			return r.ToList();
		}

		public static List<int> Mentions(this Status tweet)
		{
			var r = tweet.Entities.
						 UserMentionEntities.
						 SelectMany((UserMentionEntity arg) =>
			 new int[] { arg.Start, arg.End - arg.Start });
			return r.ToList();
		}

		public static List<int> Hashtags(this Status tweet)
		{
			var r = tweet.Entities.
			             HashTagEntities.
			             SelectMany((HashTagEntity arg) => 
			 new int[] { arg.Start, arg.End - arg.Start });
			return r.ToList();
			//var hashtagLocations = new List<int>();
			//var text = tweet.Text;
			//int loc = 0;
			//while ((loc = text.IndexOf(Symbols.H, loc)) > 0)
			//{
			//	hashtagLocations.Add(loc);
			//	loc = text.IndexOf(Symbols.S, loc);
			//	if (loc < 0)
			//		loc = text.Length;
			//	hashtagLocations.Add(loc);
			//}
			//return hashtagLocations;
		}
	}
}
