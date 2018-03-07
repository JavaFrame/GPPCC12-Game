using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Rigidbody))]
public class RtsPlayer : Player
{
	public Camera camera;

	public float zoomSensitivity = 10;
	public float zoomLvl = 1;
	public float maxZoomLvl = 10;
	public float zoomHeight = 50f;
    public float speed = 5f;
	public float fastSpeed = 10f;
    public float lookSensitivity = 3f;
    public float viewRange = 60f;
	private float baseHeight;

	private Rigidbody rigidbody;

	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.MovePosition(new Vector3(transform.position.x, transform.position.y + zoomHeight, transform.position.z));
		baseHeight = transform.position.y;
	}

    public override void GeneralUpdate()
    {
    }

    public override void UpdateRotation()
    {
        var yRot = Input.GetAxisRaw("Mouse X");
        var rotation = new Vector3(0, yRot, 0) * lookSensitivity;
        transform.Rotate(rotation);
    }

    public override void UpdateCameraRotation()
    {
        var xRot = Input.GetAxisRaw("Mouse Y");
        var cameraRotation = new Vector3(xRot, 0, 0) * lookSensitivity;
        if (camera == null) return;
        camera.transform.Rotate(-cameraRotation);
        camera.transform.localEulerAngles =
            new Vector3(ClampAngle(camera.transform.localEulerAngles.x, -viewRange, viewRange), 0, 0);

	    float scrollWheel = Input.GetAxisRaw("ScrollWheel");
	    zoomLvl += -scrollWheel * zoomSensitivity;
	    if (zoomLvl <= 1)
		    zoomLvl = 1;
		else if (zoomLvl >= maxZoomLvl)
		    zoomLvl = maxZoomLvl;

	    var transPos = rigidbody.position;
		//transform.position = new Vector3(transPos.x, baseHeight + zoomHeight * zoomLvl * zoomSensitivity, transPos.z);
		rigidbody.MovePosition(new Vector3(transPos.x, baseHeight + zoomHeight * zoomLvl * zoomSensitivity, transPos.z));
		Debug.Log(String.Format("ZoomLvl: {0}, y: {1}", zoomLvl, transform.position.y));
    }

    public override void UpdateMovement()
    {
        var xMov = Input.GetAxisRaw("Horizontal") * transform.right;
        var zMov = Input.GetAxisRaw("Vertical") * transform.forward;

	    var speed = this.speed;
	    if (Input.GetButton("Run"))
		    speed = fastSpeed;
        var velocity = (xMov + zMov).normalized * speed * zoomLvl;
        if (velocity != Vector3.zero)
            transform.position = transform.position + velocity * Time.fixedDeltaTime;
    }

}
