using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class Player : MonoBehaviour
{
	protected Rigidbody rigidbody;
    protected Collider colider;
    protected Camera camera;

    private float distToGround;


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
        UpdateRotation();
        UpdateCameraRotation();
        UpdateMovement();
        GeneralUpdate();
	}

    public abstract void GeneralUpdate();

    public abstract void UpdateRotation();

    public abstract void UpdateCameraRotation();

    public abstract void UpdateMovement();

	public float ClampAngle(float angle, float min, float max)
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
