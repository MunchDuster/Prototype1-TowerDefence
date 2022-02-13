using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
	public EnergySource start;
	public EnergySource end;
	public float pointsPerMeter = 3;
	public float downPull = 0.1f;

	private LineRenderer lineRenderer;

	// Start is called before the first frame update
	private void Awake()
	{
		start.onChanged += end.RecalculateEnergy;
	}

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		UpdateLine();
	}

	void UpdateLine()
	{
		float distance = (start.transform.position - end.transform.position).magnitude;

		int pointsLength = Mathf.RoundToInt(distance * pointsPerMeter);

		Vector3[] points = new Vector3[pointsLength];

		Vector3 downPoint = Vector3.Lerp(start.transform.position, end.transform.position, 0.5f) - Vector3.up * distance * downPull;

		for (int i = 0; i < pointsLength; i++)
		{
			float percent = (float)i / (pointsLength - 1);
			points[i] = BezierCurve(start.transform.position, end.transform.position, downPoint, percent);
		}


		lineRenderer.positionCount = pointsLength;
		lineRenderer.SetPositions(points);
	}

	Vector3 BezierCurve(Vector3 start, Vector3 end, Vector3 anchor, float percent)
	{
		Vector3 A = Vector3.Lerp(start, anchor, percent);
		Vector3 B = Vector3.Lerp(anchor, end, percent);
		Vector3 C = Vector3.Lerp(A, B, percent);

		return C;
	}
}
