using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
	public float damage = 20;
	public float speed = 20;

	private Rigidbody rb;


	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
	}

	// OnCollisionEnter is called when a collider on this gameobject collides with another
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.gameObject.tag == "Enemy")
		{
			Bug bug = collision.collider.gameObject.GetComponentInParent<Bug>();
			if (bug != null)
			{
				bug.TakeDamage(damage);
			}
		}
		Destroy(gameObject);
	}
}
