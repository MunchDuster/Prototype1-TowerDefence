using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
	public delegate void TriggerEvent(Collider collider);
	public TriggerEvent onTriggerEnter;
	public TriggerEvent onTriggerExit;

	private void OnTriggerEnter(Collider collider)
	{
		if (onTriggerEnter != null) onTriggerEnter(collider);
	}

	private void OnTriggerExit(Collider collider)
	{
		if (onTriggerExit != null) onTriggerExit(collider);
	}
}