using UnityEngine;

public class ConveyorItem : MonoBehaviour
{
	public int pointIndex = 0;
	public float totalDist = 0;
	
	public bool isWaiting = false;
	public int waitIndex = 0;

	public bool isFuel;

	public ConveyorItem Copy()
	{
		return MemberwiseClone() as ConveyorItem;
	}
}