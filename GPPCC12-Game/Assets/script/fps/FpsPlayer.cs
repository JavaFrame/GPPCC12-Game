using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FpsPlayer : MonoBehaviour
{
	public float speed = 5f;
	public float lookSensitivity = 3f;
	public float viewRange = 60f;

	public int maxJumpCounter = 1;
	public int jumpCounter = 0;
	private float distToGround;

	private Rigidbody rigidbody;
	private Collider colider;
	public Camera camera;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultipler = 2f;

	// Use this for initialization
	void Start ()
	{
		rigidbody = GetComponent<Rigidbody>();
		colider = GetComponent<Collider>();
		distToGround = colider.bounds.extents.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		var xMov = Input.GetAxisRaw("Horizontal") * transform.right;
		var zMov = Input.GetAxisRaw("Vertical") * transform.forward;

		var velocity = (xMov + zMov).normalized * speed;
		if(velocity != Vector3.zero)
			rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

		var yRot = Input.GetAxisRaw("Mouse X");
		var rotation = new Vector3(0, yRot, 0) * lookSensitivity;
		rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(rotation));

		var xRot = Input.GetAxisRaw("Mouse Y");
		var cameraRotation = new Vector3(xRot, 0, 0) * lookSensitivity;
		if (camera != null)
		{
			camera.transform.Rotate(-cameraRotation);
			camera.transform.localEulerAngles =
				new Vector3(ClampAngle(camera.transform.localEulerAngles.x, -viewRange, viewRange), 0, 0);
		}

		if (Input.GetButtonDown("Jump") && jumpCounter < maxJumpCounter)
		{
			jumpCounter++;
			rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}

		if (IsOnGround())
		{
			jumpCounter = 0;
		}
	}

	private float ClampAngle(float angle, float min, float max)
	{
 
		if (angle < 90 || angle > 270)
		{       // if angle in the critic region...
			if (angle > 180) angle -= 360;  // convert all angles to -180..+180
			if (max > 180) max -= 360;
			if (min > 180) min -= 360;
		}
		angle = Mathf.Clamp(angle, min, max);
		if (angle < 0) angle += 360;  // if angle negative, convert to 0..360
		return angle;
	}

	public bool IsOnGround()
	{
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}
}
