using UnityEngine;

public abstract class ItemTransport: PlaceObject
{
	public ItemTransport next;

	public abstract bool CanTakeItem(ConveyorItem item);
	public abstract void TakeItem(ConveyorItem item);
	public abstract void GiveItem(ConveyorItem item, ItemTransport to);

	public delegate void OnEvent();
	public OnEvent OnSpaceCleared;

	public abstract ConveyorItem[] GetItems();
}