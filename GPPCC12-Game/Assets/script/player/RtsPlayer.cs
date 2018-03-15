using System;
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

	/// <summary>
	/// If the cursor is set to invisble then the position is saved in this variable to be restored, once the cursor is not invisible anymore
	/// </summary>
	private Vector2 tempMousePos = Vector2.zero;

    void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.MovePosition(new Vector3(transform.position.x, transform.position.y + zoomHeight, transform.position.z));
		baseHeight = transform.position.y;
        Cursor.visible = true;

        // Tell your Player what your Base is(For Base Gui, Spawning of Units, etc...)
        GameObject baseObject = GameObject.Find("CoreSphere");
        if (baseObject != null)
        {
            UiReferrer _uiReferrer = baseObject.GetComponent<UiReferrer>();
            GameObject baseGui = this.transform.GetChild(2).gameObject;
            _uiReferrer.canvasGo = baseGui;

            GameObject trainDpsUnitGUI = baseGui.transform.GetChild(0).gameObject.transform.GetChild(5).gameObject;
            GameObject trainTankUnitGUI = baseGui.transform.GetChild(0).gameObject.transform.GetChild(6).gameObject;
            GameObject trainHealingUnitGUI = baseGui.transform.GetChild(0).gameObject.transform.GetChild(7).gameObject;

            Button btnTrainDpsUnit = trainDpsUnitGUI.transform.GetChild(5).gameObject.GetComponent<Button>();
            Button btnTrainTankUnit = trainTankUnitGUI.transform.GetChild(5).gameObject.GetComponent<Button>();
            Button btnTrainHealingUnit = trainHealingUnitGUI.transform.GetChild(5).gameObject.GetComponent<Button>();

            SpawnRtsUnit spawnRtsUnit = baseObject.GetComponent<SpawnRtsUnit>();
            spawnRtsUnit.spawner = GetComponent<Spawner>();

            if (spawnRtsUnit != null)
            {
                btnTrainDpsUnit.onClick.RemoveAllListeners();
                btnTrainDpsUnit.onClick.AddListener(() => spawnRtsUnit.SpawnUnit("DpsUnit"));

                btnTrainTankUnit.onClick.RemoveAllListeners();
                btnTrainTankUnit.onClick.AddListener(() => spawnRtsUnit.SpawnUnit("TankUnit"));

                btnTrainHealingUnit.onClick.RemoveAllListeners();
                btnTrainHealingUnit.onClick.AddListener(() => spawnRtsUnit.SpawnUnit("HealingUnit"));
            }
        }
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
