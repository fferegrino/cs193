using System;
using Foundation;
using UIKit;

namespace DropIt
{
	public class FallingObjectBehavior : UIDynamicBehavior
	{

		public UIGravityBehavior gravity = new UIGravityBehavior();
		UICollisionBehavior _collider;
		UICollisionBehavior collider => _collider ?? (_collider = new UICollisionBehavior
		{ TranslatesReferenceBoundsIntoBoundary = true });
		UIDynamicItemBehavior _itemBehavior;
		UIDynamicItemBehavior itemBehavior => _itemBehavior ?? (_itemBehavior = new UIDynamicItemBehavior
		{ AllowsRotation = false,
			Elasticity = 0.75f});

		public void AddBarrier(UIBezierPath barrier, string name)
		{
			var id = new NSString(name);
			collider.RemoveBoundary(id);
			collider.AddBoundary(id, barrier);
		}

		public FallingObjectBehavior()
		{
			AddChildBehavior(gravity);
			AddChildBehavior(collider);
			AddChildBehavior(itemBehavior);
		}

		public void AddItem(IUIDynamicItem item)
		{
			gravity.AddItem(item);
			collider.AddItem(item);
			itemBehavior.AddItem(item);
		}

		public void RemoveItem(IUIDynamicItem item)
		{
			gravity.RemoveItem(item);
			collider.RemoveItem(item);
			itemBehavior.RemoveItem(item);
		}
	}
}
