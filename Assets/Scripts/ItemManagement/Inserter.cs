using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inserter : ItemTransport
{
	
	public Animator animator;
	public Transform claw;
	public float takeDistance = 0.1f;

	public ConveyorItem item;

	public Vector3 takePlace;
	public Vector3 putPlace;

	ItemTransport prev;

	Vector3 takePosition {get {return transform.TransformPoint(takePlace);}}
	Vector3 putPosition {get {return transform.TransformPoint(putPlace);}}

	// Awake is called when the gameObject is activated
	private void Awake()
	{
		PlaceObject putObject = PlaceObject.ObjectAtPlace(putPosition);
		Debug.Log(putObject);
	}

	protected override Vector2Int[] GetWorldIndices()
	{
		return new Vector2Int[1] { GetWorldIndex(transform.position)};
	}

	// FixedUpdate is called every physics update
	private void FixedUpdate()
	{
		if(!item)
		{
			foreach(ConveyorItem item in prev.GetItems())
			{
				float distance = Vector3.Distance(item.transform.position, claw.position);

				if(distance <= takeDistance)
				{
					Debug.Log("Taking item");
					prev.GiveItem(item, this);
					break;
				}
			}
		}
		else
		{
			item.transform.position = claw.position;
		}
	}

	public void GiveEvent()
	{
		GiveItem(item, next);
	}

	public override void TakeItem(ConveyorItem item)
	{
		animator.SetTrigger("Insert");
		this.item = item;	
	}

	public override void GiveItem(ConveyorItem item, ItemTransport to)
	{
		animator.ResetTrigger("Insert");
		to.TakeItem(this.item);
		this.item = null;
	}

	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(takePosition, 0.2f);
		Gizmos.DrawWireSphere(putPosition, 0.2f);
	}
	#endif

	public override bool CanTakeItem(ConveyorItem item)
	{
		return next.CanTakeItem(item);
	}

	public override ConveyorItem[] GetItems()
	{
		return item ? new ConveyorItem[1] {item} : new ConveyorItem[0];
	}
}
