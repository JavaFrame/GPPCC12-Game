using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FpsPlayer : Player
{
	protected Rigidbody _rigidbody;
	protected Collider _colider;
	public Camera _camera;
	public Animator _animator;

	/// <summary>
	/// the normal walking speed of the player
	/// </summary>
	public float speed = 5f;

	/// <summary>
	/// The speed which is used when running
	/// </summary>
	public float runningSpeed = 10f;

	/// <summary>
	/// How sensitiv the _camera should response to the mous movement
	/// </summary>
	public float lookSensitivity = 3f;
	/// <summary>
	/// How much "up" or "down" the player can look in degrees (for down the viewRange is just negated)
	/// </summary>
	public float viewRange = 60f;


    //jumping stuff
	/// <summary>
	/// the force of the jump
	/// </summary>
    public float jumpForce = 2.5f;

	/// <summary>
	/// not implemented yet, so no fucking idea
	/// </summary>
	public float fallMultiplier = 2.5f;

	/// <summary>
	/// also not implemented yet, so also no fucking idea
	/// </summary>
	public float lowJumpMultipler = 2f;

	/// <summary>
	/// how many times the player can jump. 0 = 1 jump, 1 = 2 jump
	/// </summary>
    public int maxJumpCounter = 1;

	/// <summary>
	/// the counter of how many times the player already jumped. It is reseted if the player touches something below of him
	/// </summary>
    public int jumpCounter = 0;

	/// <summary>
	/// a threashold in which the compontent recogine it as touched ground
	/// </summary>
    private float distToGround;


	/// <summary>
	/// The weapon of the player
	/// </summary>
	public Weapon weapon;

	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_colider = GetComponent<Collider>();
		distToGround = _colider.bounds.extents.y;
		Cursor.lockState = CursorLockMode.Locked;
	}

    public override void GeneralUpdate()
    {
        if (Input.GetButtonDown("Jump") && jumpCounter < maxJumpCounter)
        {
            jumpCounter++;
            //_rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            _rigidbody.AddForce(Vector3.up * jumpForce);
        }

        if (IsOnGround())
        {
            jumpCounter = 0;
        }

	    if (Input.GetButton("Shoot") && weapon.Use())
	    {
			_animator.SetTrigger("shooting");
	    }
    }

    public override void UpdateRotation()
    {
        var yRot = Input.GetAxisRaw("Mouse X");
        var rotation = new Vector3(0, yRot, 0) * lookSensitivity;
        _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(rotation));
    }

    public override void UpdateCameraRotation()
    {
        var xRot = Input.GetAxisRaw("Mouse Y");
        var cameraRotation = new Vector3(xRot, 0, 0) * lookSensitivity;
	    if (_camera != null)
	    {
		    _camera.transform.Rotate(-cameraRotation);
		    _camera.transform.localEulerAngles =
			    new Vector3(ClampAngle(_camera.transform.localEulerAngles.x, -viewRange, viewRange), 0, 0);
	    }

    }

    public override void UpdateMovement()
    {
        var xMov = Input.GetAxisRaw("Horizontal") * transform.right;
        var zMov = Input.GetAxisRaw("Vertical") * transform.forward;

        var velocity = (xMov + zMov).normalized * speed;
        if (velocity != Vector3.zero) 
            _rigidbody.MovePosition(_rigidbody.position + velocity * Time.fixedDeltaTime);

		_animator.SetBool("walking", velocity != Vector3.zero);

	    _animator.SetFloat("x", Input.GetAxisRaw("Horizontal"));
	    _animator.SetFloat("y", Input.GetAxisRaw("Vertical"));
	}

	/// <summary>
	/// Checks wheter the player is on a ground. 
	/// </summary>
	/// <returns>if the player touches the ground or not</returns>
	public bool IsOnGround()
	{
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

}
