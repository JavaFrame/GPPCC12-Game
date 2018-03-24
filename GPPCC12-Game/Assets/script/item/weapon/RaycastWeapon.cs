using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : Weapon
{
	[SerializeField]
	private float _range;

    [SerializeField]
    private ParticleSystem particleSystem;

	public float Range
	{
		get { return _range; }
		set { _range = value; }

	}

	// Use this for initialization
	void Start () {
		TriggerEvent += OnTriggerEvent;	
	}

	private void OnTriggerEvent(Weapon weapon)
	{
        if (particleSystem != null)
            particleSystem.Play();
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.rotation.eulerAngles, out hit, _range))
		{
			GameObject victim = hit.collider.gameObject;
			Hurtable hurtable = victim.GetComponent<Hurtable>();
			if (hurtable != null)
			{
				hurtable.Damaged(this.damage, this.gameObject, this);
				Hitted((victim));
			}
		}
	}
}
