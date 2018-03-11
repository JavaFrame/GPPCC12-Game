using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{

	// Update is called once per frame
	void Update ()
	{
        UpdateRotation();
        UpdateCameraRotation();
        UpdateMovement();
        GeneralUpdate();
	}

	/// <summary>
	/// A general update function. It is called in every Update function and should be used for stuff which doesn't fit the 
	/// other function's descriptions.
	/// </summary>
    public abstract void GeneralUpdate();

	/// <summary>
	/// A function in which the roation of the player should be updated. It is called every Update()
	/// </summary>
    public abstract void UpdateRotation();

	/// <summary>
	/// A function in which the roation of the _camera of the player should be updated. It is called every Update()
	/// </summary>
    public abstract void UpdateCameraRotation();

	/// <summary>
	/// A function in which the movement of the player should be handeled. It is called every Update()
	/// </summary>
    public abstract void UpdateMovement();

	/// <summary>
	/// Clamps the angle between the given min and max.
	/// </summary>
	/// <param name="angle">the given angle which sould be clamped</param>
	/// <param name="min">the min</param>
	/// <param name="max">the max</param>
	/// <returns>the clamped angle</returns>
	public static float ClampAngle(float angle, float min, float max)
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
}
