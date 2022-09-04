using UnityEngine;

public class PlayerHover : MonoBehaviour
{
	public float maxRaycastDistance = 10;
	public float maxHoverDistance = 5;
	public Transform hoverPoint;
	public LayerMask layerMask;

	public delegate void OnHoverEvent(GameObject obj, RaycastHit hit);
	public OnHoverEvent OnHoverObject;
	public OnHoverEvent HoverUpdate;

	private OnHoverControl lastHover;
	private GameObject lastHoverObject;
	//Start is called before first update.
	private void Start()
	{
		OnHoverObject += CheckForHoverControl;
	}

	//Update is called every frame.
	private void Update()
	{
		if (Physics.Raycast(hoverPoint.position, hoverPoint.forward, out RaycastHit hit, maxRaycastDistance, layerMask, QueryTriggerInteraction.Ignore))
		{
			//Calling OnHoverObject and HoverUpdate event
			GameObject hoverObject = hit.collider.gameObject;

			if (HoverUpdate != null) HoverUpdate(hoverObject, hit);

			if (hoverObject != lastHoverObject)
			{
				OnHoverObject(hoverObject, hit);
				lastHoverObject = hoverObject;
			}
		}
		else
		{
			if (HoverUpdate != null) HoverUpdate(null, hit);
		}
	}

	private void CheckForHoverControl(GameObject obj, RaycastHit hit)
	{
		//Checking for OnHoverControl on object
		if (hit.distance >= maxHoverDistance)
		{
			return;
		}

		OnHoverControl hoverItem = obj.GetComponentInParent<OnHoverControl>();

		if (lastHover != null) lastHover.LeaveHover();

		if (hoverItem == null)
		{
			return;
		}

		hoverItem.EnterHover();
		lastHover = hoverItem;
	}
}