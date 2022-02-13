using UnityEngine;
using UnityEngine.Events;

public class OnHoverControl : MonoBehaviour
{
	public UnityEvent OnHoverEnter;
	public UnityEvent OnHoverEnd;


	private Outline outline;

	// Start is called before the first frame update
	private void Awake()
	{
		outline = gameObject.AddComponent<Outline>();

		outline.OutlineColor = new Color(1, 0.5f, 0);
		outline.OutlineWidth = 10;
		outline.enabled = false;
	}

	public void EnterHover()
	{
		if (OnHoverEnter != null) OnHoverEnter.Invoke();
		outline.enabled = true;
	}

	public void LeaveHover()
	{
		if (OnHoverEnd != null) OnHoverEnd.Invoke();
		outline.enabled = false;
	}
}
