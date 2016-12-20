namespace Smashtag
{
	public enum MetaDataKind
	{
		Image,
		Mention,
		Hashtag,
		Link
	}
	public class MetaDataElement
	{
		public string Value
		{
			get;
			set;
		}

		public MetaDataKind Kind
		{
			get;
			set;
		}
		public decimal AspectRatio
		{
			get;
			set;
		}
	}
}