using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : ItemTransport
{
    public float speed = 1;
    
    [Space(10)]
    public Transform startPoint;
	public Transform endPoint;
    
    [Space(10)]
    public List<ConveyorItem> items;

    private float backupDist;    

    private float length;
	private int maxObjects;
	private float maxDist {get { return length - backupDist; }}
	private bool waitingForSpace;
    
    // Start is called before the first frame update
    private void Start()
    {
		//Set all conveyorparts animatin sped
		foreach(Animator animator in GetComponentsInChildren<Animator>())
		{
			animator.SetFloat("Speed", speed);
		}

		//Check if can specialize next if conveyorbelt
		if(next != null)
		{
			ConveyorBelt belt = next as ConveyorBelt;
			if(belt != null)
			{
				belt.startPoint.position = endPoint.position;
			}
		}


		length = Vector3.Distance(startPoint.position, endPoint.position);

		maxObjects = (int)(length * 2);

        for(int i = 0; i < items.Count; i++)
        {
            float distance = 0.5f * i;
            items[i].totalDist = distance;
            
            Vector3 point = GetPointAtDistance(distance);
            items[i].transform.position = point;
        }
    }

	protected override Vector2Int[] GetWorldIndices()
	{
		List<Vector2Int> points = new List<Vector2Int>();

		Vector3 start = startPoint
	}
    
    // Update is called every frame update
    private void FixedUpdate()
    {
        for(int i = 0; i < items.Count; i++)
        {
            UpdateItem(items[i]);
        }

		if(OnSpaceCleared != null)
		{
			bool spaceAvailable = items.Count > 0 && items[items.Count - 1].totalDist > 0.5f;
			if(spaceAvailable) OnSpaceCleared();
		}
	}

	public override bool CanTakeItem(ConveyorItem item)
	{
		if(items.Count >= maxObjects)
		{
			Debug.Log("TOO MANY OBJECTS");
			return false;
		}
		if(items.Count > 0 && items[items.Count - 1].totalDist < 0.5f)
		{
			Debug.Log("NOT ENOUGH SPACE");
			return false;
		}
		return true;
	}
    
    public override void TakeItem(ConveyorItem item)
    {
        items.Add(item.Copy());
    }

	public override ConveyorItem[] GetItems()
	{
		return items.ToArray();
	}
	
    
    private void UpdateItem(ConveyorItem item)
    {
        if(!item.isWaiting)
        {
            item.totalDist += Time.fixedDeltaTime * speed;
            
            if(item.totalDist >= maxDist)
            {
                if(next != null)
				{
					if(item.totalDist >= length)
					{
						Debug.Log("Giving item");
						GiveItem(item, next);
					}
				}
                else 
                {
                    SetItemWaiting(item);
                }
				return;
            }
            
            item.transform.position = GetPointAtDistance(item.totalDist);
        }
    }

	public override void GiveItem(ConveyorItem item, ItemTransport to)
	{
		item.totalDist = item.totalDist % length;

		bool itemAccepted = to.CanTakeItem(item);
		if(itemAccepted)
		{
			to.TakeItem(item);
			items.Remove(item);	
			backupDist = Mathf.Max(backupDist - 0.5f, 0);
			if(OnSpaceCleared != null) OnSpaceCleared();
		}
		else
		{
			if(to == next)SetItemWaiting(item);
		}
	}

	private void SetItemWaiting(ConveyorItem item)
	{
		Debug.Log("Item waiting");
        item.isWaiting = true;
		backupDist += 0.5f;
        item.totalDist = maxDist;
		if(!waitingForSpace && next != null) next.OnSpaceCleared += UnwaitAllItems;
	}

	private void UnwaitAllItems()
	{
		waitingForSpace = false;
		backupDist = 0;
		next.OnSpaceCleared -= UnwaitAllItems;
		foreach(ConveyorItem item in items)
		{
			item.isWaiting = false;
		}
	}

	//The the point along the belt at distance from start
	public Vector3 GetPointAtDistance(float distance)
	{
		float lerp = 1 - (length - distance) / length;
		return Vector3.Lerp(startPoint.position, endPoint.position, lerp);
	}
    
    //Inverse Lerp between vector3 on line
    private float InverseLerp(Vector3 target, Vector3 last, Vector3 position)
    {
        //target - C, position - B, last - A
        float CB = (target - position).magnitude;
        float CA = (target - last).magnitude;
        return CB / CA;
    }
}
