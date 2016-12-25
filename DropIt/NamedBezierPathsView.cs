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

		Dictionary<string, UIBezierPath> _bezierPaths = new Dictionary<string, UIBezierPath>(); 
		Dictionary<string, UIBezierPath> BezierPaths
		{
			get { return _bezierPaths; }
			set { _bezierPaths = value; }
		}

		public void AddBeizerPath(string name, UIBezierPath path)
		{
			if (BezierPaths.ContainsKey(name)) 
				BezierPaths.Remove(name);
			BezierPaths.Add(name, path);
			SetNeedsDisplay();
		}

		public void RemoveBezierPath(string name)
		{
			BezierPaths.Remove(name);
			SetNeedsDisplay();
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
