using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : Weapon {
	/// <summary>
	/// the speed of the projectile
	/// </summary>
	public float speed;
	/// <summary>
	/// The time until the TimeoutEvent is invoked. If the time is smaller then zero, it will be ignored
	/// </summary>
	public float time;
	/// <summary>ss
	/// the initial rotion of the projectile
	/// </summary>
	public Vector3 initRotation = Vector3.forward;

	/// <summary>
	/// The timout delegate
	/// </summary>
	/// <param name="projectile"></param>
	/// <param name="w"></param>
	public delegate void Timeout(Projectile projectile, Weapon w);
	public event Timeout TimeoutEvent;

	public delegate void Destroy(Projectile projectile);

	public event Destroy DestroyEvent;

	public bool destroyOnHit = true;

	private float startTime;
	public Weapon parentWeapon;

	private Rigidbody _rigidbody;

	private void Awake()
	{
		startTime = Time.time;
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.position += transform.forward;
		_rigidbody.velocity = transform.forward * speed;
	}

	private void Update()
	{
		if(Time.time - startTime > time && time > 0)
		{
			if(TimeoutEvent != null) TimeoutEvent.Invoke(this, parentWeapon);
			Destroy(this.gameObject, 0.4f);
			if(DestroyEvent != null) DestroyEvent.Invoke(this);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		var go = collision.gameObject;
		var hurtable = go.GetComponent<Hurtable>();
		if(hurtable != null)
		{
			if(parentWeapon == null)
				Debug.LogWarning("Parent weapon of projectile " + gameObject.name + " is null");
			hurtable.Damaged(damage, (parentWeapon!=null?parentWeapon.parent:null), parentWeapon);
			Hitted(go);
		}
		if (DestroyEvent != null) DestroyEvent.Invoke(this);
		if (destroyOnHit)
			Destroy(this.gameObject, 0.1f);
	}


}
