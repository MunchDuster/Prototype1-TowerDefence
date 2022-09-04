using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemy;
	public Transform target;
	public float rate = 1;
	public float spawnRadius = 10;

	private float timeSinceSpawned;

	// Start is called before the first frame update
	void Start()
	{
		Spawn();
	}

	// Update is called once per frame
	void Update()
	{
		timeSinceSpawned += Time.deltaTime;

		if (timeSinceSpawned >= (1 / rate))
		{
			Spawn();
		}
	}

	void Spawn()
	{
		Vector3 position = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(20f, 30f)) * spawnRadius;
		GameObject instantiation = Instantiate(enemy, position, Quaternion.identity, transform);

		instantiation.GetComponent<Bug>().target = target;

		timeSinceSpawned = 0;
	}
}
