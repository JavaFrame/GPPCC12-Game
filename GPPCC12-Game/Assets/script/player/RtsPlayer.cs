﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

/// <summary>
/// Implements the Rts player movement logic
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Spawner))]
public class RtsPlayer : Player
{
	/// <summary>
	/// the _camera which has to be set in the editor
	/// </summary>
	public Camera _camera;

	/// <summary>
	/// The sensitivity of the scroll wheel. The output of the axis "ScrollWheel" is multiplied by this value
	/// </summary>
	public float zoomSensitivity = 10;

	/// <summary>
	/// The level of zoom. One = the nearest possible settings; maxZoomLvl = the fartest possible setting
	/// </summary>
	public float zoomLvl = 1;

	/// <summary>
	/// the maximum zoom lvl
	/// </summary>
	public float maxZoomLvl = 10;
	
	/// <summary>
	/// the height which is added removed with one zoom lvl
	/// </summary>
	public float zoomHeight = 50f;

	/// <summary>
	/// the speed of the normal flying
	/// </summary>
    public float speed = 5f;

	/// <summary>
	/// the speed of flying fast. Its used when the player holds down shift
	/// </summary>
	public float fastSpeed = 10f;

	/// <summary>
	/// How sensitiv the _camera is
	/// </summary>
    public float lookSensitivity = 3f;

	/// <summary>
	/// The maximum and minum
	/// </summary>
    public float viewRange = 60f;

	/// <summary>
	/// the hight of one scroll unit
	/// </summary>
	private float baseHeight;

	private Rigidbody _rigidbody;

    void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.MovePosition(new Vector3(transform.position.x, transform.position.y + zoomHeight, transform.position.z));
		baseHeight = transform.position.y;
        Cursor.visible = true;
    }

    public override void GeneralUpdate()
    {
    }

    public override void UpdateRotation()
    {	   
    }

    public override void UpdateCameraRotation()
    {
	    float scrollWheel = Input.GetAxisRaw("ScrollWheel");
	    zoomLvl += -scrollWheel * zoomSensitivity;
        if (zoomLvl <= 1)
		    zoomLvl = 1;
		else if (zoomLvl >= maxZoomLvl)
		    zoomLvl = maxZoomLvl;

	    var transPos = _rigidbody.position;
		_rigidbody.MovePosition(new Vector3(transPos.x, baseHeight + zoomHeight * zoomLvl * zoomSensitivity, transPos.z));
    }

    public override void UpdateMovement()
    {
        var xMov = Input.GetAxisRaw("Horizontal") * transform.right;
        var zMov = Input.GetAxisRaw("Vertical") * transform.up;

	    var speed = this.speed;
	    if (Input.GetButton("Run"))
		    speed = fastSpeed;
        var velocity = (xMov + zMov).normalized * speed * zoomLvl;
        if (velocity != Vector3.zero)
            transform.position = transform.position + velocity * Time.fixedDeltaTime;
    }
}
