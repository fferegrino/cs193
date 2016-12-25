using System;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;

namespace DropIt
{
	public class NamedBezierPathsView : UIView
	{
		public NamedBezierPathsView(IntPtr handle) : base(handle)
		{
		}

		Dictionary<string, UIBezierPath> _bezierPaths; 
		internal Dictionary<string, UIBezierPath> BezierPaths
		{
			get { return _bezierPaths; }
			set { _bezierPaths = value; SetNeedsDisplay();}
		}

		public override void Draw(CGRect rect)
		{
			foreach (var item in BezierPaths)
			{
				var path = item.Value;
				path.Stroke();
			}
		}
	}
}
