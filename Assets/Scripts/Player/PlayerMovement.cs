using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
	public float speed = 5;
	public float turnspeed = 180;
	public float jumpVelocity = 2;

	public Transform head;
	public bool lockMouse;

	private CharacterController controller;

	//Inputs
	private Vector2 move = Vector2.zero;
	private Vector2 look = Vector2.zero;
	private bool jumpPressed;

	//Movement and look vars
	private float yVelocity = 0;
	private float xAngle = 0;

	// Start is called before the first frame update
	void Start()
	{
		controller = GetComponent<CharacterController>();
		SetMouseLocked(true);
	}

	// Update is called once per frame
	void Update()
	{
		UpdateInputs();
		CalculateYVelocity();
		Move(); //move inputs
		Turn(); //look inputs
	}

	//Gets inputs from user
	private void UpdateInputs()
	{
		//WSAD or arrows
		move.x = Input.GetAxis("Horizontal");
		move.y = Input.GetAxis("Vertical");

		//Mouse movement
		look.x = Input.GetAxis("Mouse X");
		look.y = Input.GetAxis("Mouse Y");

		//Spacebar
		jumpPressed = Input.GetKeyDown(KeyCode.Space);
	}

	//Moves position of transform from move inputs
	private void Move()
	{
		Quaternion simplifiedRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

		//movement on local x and z axis
		Vector3 forwardUnit = simplifiedRotation * (Vector3.forward * move.y);
		Vector3 rightUnit = simplifiedRotation * (Vector3.right * move.x);
		float magnitude = speed * Time.deltaTime;

		//movement on Y axis
		float Ymagnitude = yVelocity * Time.deltaTime;
		Vector3 yMove = Vector3.up * Ymagnitude;

		//Apply movement on CharacterController
		controller.Move((forwardUnit + rightUnit) * magnitude + yMove);
	}

	//Rotates transform and head from look inputs
	private void Turn()
	{
		float magnitude = turnspeed * Time.deltaTime;

		//Turn head on X-axis
		xAngle = Mathf.Clamp(xAngle + look.y * magnitude, -90, 90);
		Vector3 xTurnUnit = Vector3.left * xAngle;
		head.localRotation = Quaternion.Euler(xTurnUnit);//Set the local rotation

		//Turn body on Y-axis
		Vector3 yTurnUnit = Vector3.up * look.x;
		transform.Rotate(yTurnUnit * magnitude);//Rotate on the y-axis

	}

	//Calculates velocity on y axis
	private void CalculateYVelocity()
	{
		if (controller.isGrounded)
		{
			//Jump
			if (jumpPressed)
			{
				yVelocity = jumpVelocity;
			}
			else
			{
				//Floor stick
				yVelocity = -1;

			}
		}
		//Gravitational acceleration
		else
		{
			yVelocity += -9.81f * Time.deltaTime;
		}
	}

	//Toggle whether mouse locked and hidden or unlocked and visible
	public void SetMouseLocked(bool locked)
	{
		if (locked)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}