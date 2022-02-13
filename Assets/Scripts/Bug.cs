using UnityEngine;
using UnityEngine.AI;

public class Bug : MonoBehaviour
{
	public float speed;
	public Transform center;
	public GameObject fx;

	public float health = 100;

	public void TakeDamage(float damage)
	{
		health -= damage;

		if (health <= 0)
		{
			Instantiate(fx, center.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}

	public Transform target;

	private NavMeshAgent agent;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		if (agent.velocity.magnitude == 0f)
		{
			agent.SetDestination(target.position);
		}
	}
}