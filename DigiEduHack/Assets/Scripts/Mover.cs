using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	[SerializeField] float CurrentSpeed = 2f;
	[SerializeField] float rotateSpeed = 100f;

	float directionForward = 0f;

	Rigidbody rb;
	Vector3 v_Movement;

	bool isPaused = false;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		v_Movement = transform.position;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.A) && !isPaused)
		{
			transform.Rotate((Vector3.up * -1.0f) * rotateSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.D) && !isPaused)
		{
			transform.Rotate((Vector3.up * 1.0f) * rotateSpeed * Time.deltaTime);
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			print("Interact");
		}

		if (Input.GetKey(KeyCode.W))
		{
			directionForward = 1f;
			Move();
		}

		if (Input.GetKey(KeyCode.S))
		{
			directionForward = -1f;
			Move();
		}
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		
	}

	public void SetPause(bool b)
	{
		isPaused = b;
	}

	public bool IsPaused()
	{
		return isPaused;
	}

	private void Move()
	{
		if (isPaused) return;

		v_Movement = (transform.forward * directionForward).normalized;
		transform.position += v_Movement * CurrentSpeed * Time.deltaTime;
		rb.MovePosition(transform.position.normalized);
	}
}
