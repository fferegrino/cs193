using System;
using CoreGraphics;
using Foundation;
using UIKit;
using static System.Math;

namespace Calculator
{
	public class AxesDrawer
	{
		private struct Constants
		{
			public const double HashmarkSize = 6;
		}

		UIColor color = UIColor.Blue;
		double minimumPointsPerHashmark = 40;
		double contentScaleFactor = 1; // set this from UIView's contentScaleFactor to position axes with maximum accuracy

		public AxesDrawer(UIColor color, double contentScaleFactor)
		{
			this.color = color;
			this.contentScaleFactor = contentScaleFactor;
		}

		public AxesDrawer(double contentScaleFactor)
		{
			this.contentScaleFactor = contentScaleFactor;
		}

		public AxesDrawer(UIColor color)
		{
			this.color = color;
		}

		// this method is the heart of the AxesDrawer
		// it draws in the current graphic context's coordinate system
		// therefore origin and bounds must be in the current graphics context's coordinate system
		// pointsPerUnit is essentially the "scale" of the axes
		// e.g. if you wanted there to be 100 points along an axis between -1 and 1,
		//    you'd set pointsPerUnit to 50

		public void DrawAxesInRect(CGRect bounds, CGPoint origin, double pointsPerUnit)
		{
			using (var context = UIGraphics.GetCurrentContext())
			{
				context.SaveState();
				color.SetFill();
				color.SetStroke();


				var path = new UIBezierPath();

		//			path.MoveTo(new CGPoint(x: bounds.GetMinX(), y: align(origin.y)))
		//path.addLineToPoint(CGPoint(x: bounds.maxX, y: align(origin.y)))
		//path.moveToPoint(CGPoint(x: align(origin.x), y: bounds.minY))
		//path.addLineToPoint(CGPoint(x: align(origin.x), y: bounds.maxY))
		//path.stroke()
		//drawHashmarksInRect(bounds, origin: origin, pointsPerUnit: abs(pointsPerUnit))

				context.RestoreState();
			}
		}

		private void DrawHashmarksInRect(CGRect bounds, CGPoint origin, double pointsPerUnit)
		{
			if ((origin.X >= bounds.GetMinX()) && (origin.X <= bounds.GetMaxX())
				|| ((origin.Y >= bounds.GetMinY()) && (origin.Y <= bounds.GetMaxY())))

			{
				// figure out how many units each hashmark must represent
				// to respect both pointsPerUnit and minimumPointsPerHashmark
				var unitsPerHashmark = minimumPointsPerHashmark / pointsPerUnit;

				if (unitsPerHashmark < 1)
				{
					unitsPerHashmark = Pow(10, Ceiling(Log10(unitsPerHashmark)));
				}
				else {
					unitsPerHashmark = Floor(unitsPerHashmark);
				}

				var pointsPerHashmark = pointsPerUnit * unitsPerHashmark;

				// figure out which is the closest set of hashmarks (radiating out from the origin) that are in bounds
				double startingHashmarkRadius = 1;

				if (bounds.Contains(origin))
				{
					var leftx = Max(origin.X - bounds.GetMaxX(), 0);
					var rightx = Max(bounds.GetMinX() - origin.X, 0);
					var downy = Max(origin.Y - bounds.GetMinY(), 0);
					var upy = Max(bounds.GetMaxY() - origin.Y, 0);

					startingHashmarkRadius = Min(Min(leftx, rightx), Min(downy, upy)) / pointsPerHashmark + 1;

				}

				// now create a bounding box inside whose edges those four hashmarks lie
				var bboxSize = pointsPerHashmark * startingHashmarkRadius * 2;

				var bbox = new CGRect(origin, size: new CGSize(width: bboxSize, height: bboxSize));

				// formatter for the hashmark labels
				var formatter = new NSNumberFormatter();
				formatter.MaximumFractionDigits = (nint)(-1 * Log10(unitsPerHashmark));
				formatter.MinimumIntegerDigits = 1;

				// radiate the bbox out until the hashmarks are further out than the bounds
				while (!bbox.Contains(bounds))

				{
					var label = formatter.StringFromNumber((origin.X - bbox.GetMinX()) / pointsPerUnit);
					var leftHashmarkPoint = AlignedPoint(x: bbox.GetMinX(), y: origin.Y, insideBounds: bounds);
					if (leftHashmarkPoint != null)

					{
						DrawHashmarkAtLocation(leftHashmarkPoint.Value, AnchoredText.Top("-\\(label)"));

					}
					//	if let rightHashmarkPoint = alignedPoint(x: bbox.maxX, y: origin.y, insideBounds: bounds) {
					//		drawHashmarkAtLocation(rightHashmarkPoint, .Top(label))

					//}
					//	if let topHashmarkPoint = alignedPoint(x: origin.x, y: bbox.minY, insideBounds: bounds) {
					//		drawHashmarkAtLocation(topHashmarkPoint, .Left(label))

					//}
					//	if let bottomHashmarkPoint = alignedPoint(x: origin.x, y: bbox.maxY, insideBounds: bounds) {
					//		drawHashmarkAtLocation(bottomHashmarkPoint, .Left("-\(label)"))

					//}
					//	bbox.insetInPlace(dx: -pointsPerHashmark, dy: -pointsPerHashmark)

				}
			}
		}

		private void DrawHashmarkAtLocation(CGPoint location, AnchoredText text)
		{
		//	var dx: CGFloat = 0, dy: CGFloat = 0

		//switch text {
		//		case .Left:
		//			dx = Constants.HashmarkSize / 2

		//	case .Right:
		//			dx = Constants.HashmarkSize / 2

		//	case .Top:
		//			dy = Constants.HashmarkSize / 2

		//	case .Bottom:
		//			dy = Constants.HashmarkSize / 2

		//}

		//	let path = UIBezierPath()

		//path.moveToPoint(CGPoint(x: location.x - dx, y: location.y - dy))

		//path.addLineToPoint(CGPoint(x: location.x + dx, y: location.y + dy))

		//path.stroke()


		//text.drawAnchoredToPoint(location, color: color)

	}

		enum TextAnchor
		{
			Left,
			Right,
			Top,
			Bottom
		}

		class AnchoredText
		{
			const double VerticalOffset = 3;
			const double HorizontalOffset = 6;

			private AnchoredText() { }

			public static AnchoredText Left(string text)
			{
				return new AnchoredText { Text = text, Anchor = TextAnchor.Left };
			}

			public static AnchoredText Right(string text)
			{
				return new AnchoredText { Text = text, Anchor = TextAnchor.Right };
			}

			public static AnchoredText Top(string text)
			{
				return new AnchoredText { Text = text, Anchor = TextAnchor.Top };
			}

			public static AnchoredText Bottom(string text)
			{
				return new AnchoredText { Text = text, Anchor = TextAnchor.Bottom };
			}

			public string Text
			{
				get;
				private set;
			}

			public TextAnchor Anchor
			{
				get;
				private set;
			}

			public void DrawAnchoredToPoint(CGPoint location , UIColor color)
			{
				
				var attributes = new UIStringAttributes 
				{
					Font = UIFont.PreferredFootnote,
					ForegroundColor = color
				};

				var text = new NSString(Text);
				var textRect = new CGRect(location, text.GetSizeUsingAttributes(attributes));
				var y = textRect.Y;

				switch (Anchor)
				{
					case TextAnchor.Top:
						textRect.Y += (nfloat)(textRect.Size.Height / 2 + AnchoredText.VerticalOffset);
					break;
				case TextAnchor.Left:
						textRect.X += (nfloat)(textRect.Size.Width / 2 + AnchoredText.HorizontalOffset);
					break;
				case TextAnchor.Bottom:
						textRect.Y -= (nfloat)(textRect.Size.Height / 2 + AnchoredText.VerticalOffset);
					break;
				case TextAnchor.Right:
						textRect.X -= (nfloat)(textRect.Size.Width / 2 + AnchoredText.HorizontalOffset);
					break;
			}

				text.DrawString(textRect, attributes);
				//text.drawInRect(textRect, withAttributes: attributes)

		}

		}

		// we want the axes and hashmarks to be exactly on pixel boundaries so they look sharp
		// setting contentScaleFactor properly will enable us to put things on the closest pixel boundary
		// if contentScaleFactor is left to its default (1), then things will be on the nearest "point" boundary instead
		// the lines will still be sharp in that case, but might be a pixel (or more theoretically) off of where they should be

		CGPoint? AlignedPoint(nfloat x, nfloat y, CGRect insideBounds)
		{
			var point = new CGPoint(x: Align(x), y: Align(y));
				var permissibleBounds = insideBounds;
			if (permissibleBounds.Contains(point))
				return null;
			//if let permissibleBounds = insideBounds where !CGRectContainsPoint(permissibleBounds, point) {
			//		return nil
			//}
			return point;
		}

		double Align(double x)
		{
			return Round(x * contentScaleFactor) / contentScaleFactor;
		}
}
}
