using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : ItemTransport
{
	public Transform[] places;
	public List<ConveyorItem> items = new List<ConveyorItem>();

	public List<int> freePlaces;

	// Start is called before the first frame update
	private void Start()
	{
		freePlaces = new List<int>();

		for (int i = 0; i < places.Length; i++)
		{
			freePlaces.Add(i);
		}
	}

	protected override Vector2Int[] GetWorldIndices()
	{
		return new Vector2Int[1] { GetWorldIndex(transform.position)};
	}

    public override bool CanTakeItem(ConveyorItem item)
	{
		return freePlaces.Count > 0;
	}
	public override void TakeItem(ConveyorItem item)
	{
		item.transform.position = places[freePlaces[0]].position;
		item.transform.rotation = places[freePlaces[0]].rotation;
		freePlaces.RemoveAt(0);
	}
	public override void GiveItem(ConveyorItem item, ItemTransport to) {}

	public delegate void OnEvent();
	public OnEvent OnSpaceCleared;

	public override ConveyorItem[] GetItems() {return items.ToArray();}
}
