// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

namespace Smashtag
{
	public partial class RecentSearchesViewController : UITableViewController
	{
		public RecentSearchesViewController (IntPtr handle) : base (handle)
		{
		}

		struct StoryboardId
		{
			public const string RecentSearchCell = "Recent Search Cell";
		}

		struct OtherId
		{
			public const string RecentSearches = "RecentSearches";
		}

		RecentSearches _recentSearches;
		RecentSearches RecentSearches
		{
			get
			{
				if (_recentSearches == null)
				{
					_recentSearches = new RecentSearches();
				}
				return _recentSearches;
			}
		}

		string[] recent;
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			var plist = NSUserDefaults.StandardUserDefaults;
			var searches = plist.StringForKey(OtherId.RecentSearches);
			RecentSearches.Load(searches);
			recent = RecentSearches.Recent;
			TableView.ReloadData();
		}

		#region Table
		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return recent?.Length ?? 0;
		}


		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = TableView.DequeueReusableCell(StoryboardId.RecentSearchCell, indexPath);

			cell.TextLabel.Text = recent[indexPath.Row];

			return cell;
		}
		#endregion
	}
}
