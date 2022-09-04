using UnityEngine;
using UnityEngine.Events;

public class PlaceObjects : MonoBehaviour
{
	[System.Serializable]
	public struct PlaceObject
	{
		public GameObject prefab;
		public GameObject preview;
	}


	public PlayerHover playerHover;
	public float rotateSpeed = 180;

	public UnityEvent OnBuildModeEnter;
	public UnityEvent OnBuildModeExit;

	public PlaceObject[] placeables;

	private int placeableIndex = 0;
	private bool isPlacingObjects = false;
	private float yRotation = 0;

	private PlaceObject placeObject { get { return placeables[placeableIndex]; } }

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called every frame
	private void Update()
	{
		//Toggle build mode if pressing B key
		if (Input.GetKeyDown(KeyCode.B))
		{
			isPlacingObjects = !isPlacingObjects;

			if (isPlacingObjects)
			{
				OnBuildModeEnter.Invoke();
				playerHover.HoverUpdate += OnUpdate;
			}
			else
			{
				OnBuildModeExit.Invoke();
				playerHover.HoverUpdate -= OnUpdate;
				placeObject.preview.SetActive(false);
			}
		}

		if (isPlacingObjects)
		{
			//Place if left clicking
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				PlaceItem();
			}

			//Rotate if Q or E pressed
			UpdateRotation();
		}
	}

	Vector3 position;
	Quaternion rotation;

	private void PlaceItem()
	{
		Instantiate(placeObject.prefab, position, rotation);
	}

	private void UpdateRotation()
	{
		float delta = 0;
		if (Input.GetKey(KeyCode.Q)) delta++;
		if (Input.GetKey(KeyCode.E)) delta--;

		yRotation += delta * Time.deltaTime * rotateSpeed;
	}


	private void OnUpdate(GameObject obj, RaycastHit hit)
	{
		if (obj == null)
		{
			placeObject.preview.SetActive(false);
		}
		else
		{
			placeObject.preview.SetActive(true);

			Vector3 forward = Quaternion.Euler(0, yRotation, 0) * Vector3.forward;

			position = hit.point;
			rotation = Quaternion.LookRotation(forward, hit.normal);

			placeObject.preview.transform.position = position;
			placeObject.preview.transform.rotation = rotation;
		}
	}


}
