using System;
using UIKit;

namespace DropIt
{
	public class FallingObjectBehavior : UIDynamicBehavior
	{

		UIGravityBehavior gravity = new UIGravityBehavior();
		UICollisionBehavior _collider;
		UICollisionBehavior collider => _collider ?? (_collider = new UICollisionBehavior
		{ TranslatesReferenceBoundsIntoBoundary = true });
		UIDynamicItemBehavior _itemBehavior;
		UIDynamicItemBehavior itemBehavior => _itemBehavior ?? (_itemBehavior = new UIDynamicItemBehavior
		{ AllowsRotation = false,
			Elasticity = 0.75f});

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
