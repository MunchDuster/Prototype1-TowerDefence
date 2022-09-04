using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Furnace : ItemTransport
{
	public string resourceItemName;
	public Transform outputPlace;
	public GameObject craftItemPrefab;
	public ConveyorItem resourceItem;
	public ConveyorItem craftItem;
	public ConveyorItem fuelItem;

	public float craftTime = 2;
	public bool isCrafting = false;

	public UnityEvent OnStartMakeItem;
	public UnityEvent OnStopMakeItem;


	protected override Vector2Int[] GetWorldIndices()
	{
		return new Vector2Int[4] {
			GetWorldIndex(transform.position + transform.TransformPoint(new Vector3(0, 0, 0))),
			GetWorldIndex(transform.position + transform.TransformPoint(new Vector3(0, 0, 1))),
			GetWorldIndex(transform.position + transform.TransformPoint(new Vector3(1, 0, 0))),
			GetWorldIndex(transform.position + transform.TransformPoint(new Vector3(1, 0, 1))),
		};
	}

	public override bool CanTakeItem(ConveyorItem item)
	{
		if(item.isFuel) return fuelItem == null;
		else return resourceItem == null && item.name == resourceItemName;
	}
	public override void TakeItem(ConveyorItem item)
	{
		if(item.isFuel) fuelItem = item;
		else resourceItem = item;

		if(resourceItem != null && fuelItem != null)
		{
			StartCoroutine(Craft());
		}
	}
	IEnumerator Craft()
	{
		isCrafting = true;
		OnStartMakeItem.Invoke();
		yield return new WaitForSeconds(craftTime);
		OnStopMakeItem.Invoke();
		isCrafting = false;

		//Destroy old items
		Destroy(resourceItem.gameObject);
		Destroy(fuelItem.gameObject);

		//Create crafted item
		GameObject instantiation = Instantiate(craftItemPrefab, outputPlace.position, outputPlace.rotation);
		craftItem = instantiation.GetComponent<ConveyorItem>();
	}
	public override void GiveItem(ConveyorItem item, ItemTransport to)
	{
		bool itemAccepted = to.CanTakeItem(craftItem);
		if(itemAccepted)
		{
			to.TakeItem(craftItem);
			craftItem = null;
		}
	}

	public override ConveyorItem[] GetItems() 
	{
		return craftItem ? new ConveyorItem[1]{craftItem}: new ConveyorItem[0];
	}
}