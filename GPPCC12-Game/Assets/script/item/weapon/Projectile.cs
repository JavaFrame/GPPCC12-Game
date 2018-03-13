using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : Weapon {
	public float speed;
	public float time;
	public Vector3 initRotation = Vector3.forward;
	public delegate void Timeout(Projectile projectile, Weapon w);
	public event Timeout TimeoutEvent;

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
		if(Time.time -startTime > time && time > 0)
		{
			if(TimeoutEvent != null)
				TimeoutEvent.Invoke(this, parentWeapon);
			Destroy(this.gameObject, 0.4f);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		var go = collision.gameObject;
		var hurtable = go.GetComponent<Hurtable>();
		if(hurtable != null)
		{
			hurtable.Damaged(Damage, parentWeapon.parent, parentWeapon);
			Hitted(go);
		}
		if (TimeoutEvent != null)
			TimeoutEvent.Invoke(this, parentWeapon);
		Destroy(this.gameObject, 0.1f);
	}


}
