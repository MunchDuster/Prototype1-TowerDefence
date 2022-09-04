using UnityEngine;
using System.Collections.Generic;

public class Turret : MonoBehaviour
{
	//Public vars
	public float energyConsumption = 1;
	public float turnSpeed = 180;
	public float maxXAngle = 20;
	public float minXAngle = -10;

	public float minAimOffset = 0.1f;

	public Transform yRotation;
	public Transform xRotation;

	public Transform bulletPoint;

	public GameObject bulletPrefab;

	public float recoilTime;

	public AreaTrigger areaTrigger;

	//Private vars
	private float lastFireTime;

	private List<Bug> enemiesInRange = new List<Bug>();

	private float xAngle;
	private float yAngle;

	// Start is called before the first frame update
	void Start()
	{
		areaTrigger.onTriggerEnter += OnTriggerEnter;
		areaTrigger.onTriggerExit += OnTriggerExit;
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Enemy")
		{
			Bug bug = collider.gameObject.GetComponent<Bug>();
			if (bug != null)
			{
				enemiesInRange.Add(bug);
			}
		}
	}
	private void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.tag == "Enemy")
		{
			Bug bug = collider.gameObject.GetComponent<Bug>();
			if (bug != null)
			{
				if (enemiesInRange.Contains(bug))
				{
					enemiesInRange.Remove(bug);
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (enemiesInRange.Count == 0) return;

		Bug enemy = FindClosestEnemy();

		if (enemy == null) return;

		Vector3 targetPoint = CalculateEnemyTargetPosition(enemy);

		Debug.DrawRay(targetPoint, Vector3.up);

		RotateTowards(targetPoint);

		if (IsAimingAtTarget(targetPoint) && CanFire())
		{
			Fire();
		}
	}

	private bool IsAimingAtTarget(Vector3 targetPoint)
	{
		float x = yRotation.InverseTransformPoint(targetPoint).x;
		float y = xRotation.InverseTransformPoint(targetPoint).y;

		float offset = new Vector2(x, y / 2).magnitude;

		float distance = (transform.position - targetPoint).magnitude;


		return offset < minAimOffset || distance < 5;
	}
	private bool CanFire()
	{
		return Time.time - lastFireTime > recoilTime;
	}

	private void Fire()
	{
		lastFireTime = Time.time;
		Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation, transform);
	}

	private Bug FindClosestEnemy()
	{
		float smallestDistance = 100000;
		Bug closestEnemy = null;

		for (int i = 0; i < enemiesInRange.Count; i++)
		{
			Bug enemy = enemiesInRange[i];

			if (enemy == null)
			{
				enemiesInRange.Remove(enemy);
				continue;
			}

			float distance = (transform.position - enemy.transform.position).magnitude;

			if (distance < smallestDistance)
			{
				closestEnemy = enemy;
				smallestDistance = distance;
			}
		}

		return closestEnemy;
	}

	private Vector3 CalculateEnemyTargetPosition(Bug enemy)
	{
		return enemy.center.position;
	}

	private void RotateTowards(Vector3 targetPoint)
	{
		float x = yRotation.InverseTransformPoint(targetPoint).x;
		float y = -xRotation.InverseTransformPoint(targetPoint).y;

		xAngle += Mathf.Clamp(y, -1f, 1f) * turnSpeed * Time.deltaTime;
		yAngle += Mathf.Clamp(x, -1f, 1f) * turnSpeed * Time.deltaTime;

		xAngle = Mathf.Clamp(xAngle, minXAngle, maxXAngle);

		yRotation.localRotation = Quaternion.Euler(Vector3.up * yAngle);
		xRotation.localRotation = Quaternion.Euler(Vector3.right * xAngle);
	}
}
