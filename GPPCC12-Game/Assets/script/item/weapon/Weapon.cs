using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements some base weapon stuff like damage
/// </summary>
public abstract class Weapon : Item
{
	/// <summary>
	/// the damage which the wepon does.
	/// If the wepon heals, you can use negative damage
	/// </summary>
	[SerializeField]
	public int damage;

	public int maxBulletsInMagasin = -1;
	private int bulletsInMagasin;

	public int maxMagasins = -1;
	private int magasins;

	public bool autoReload = true;
	
	public float shootTime = 1f;
	private float lastShootTime;

	public float reloadTime = 1f;
	private float lastReloadTime;

	/// <summary>
	/// This delegate is used when the weapon hits a hurtable component.
	/// </summary>
	/// <param name="victem">The gameobject who was hitted</param>
	/// <param name="attacker">The gameobject who hitted the victem</param>
	/// <param name="w">The weapon which was used</param>
	public delegate void Hit(GameObject victem, GameObject attacker, Weapon w);

	/// <summary>
	/// This event gets triggered if the weapon hits a hurtable component
	/// </summary>
	public event Hit HitEvent;

	/// <summary>
	/// This delegate is used when the weapon is triggered/used.
	/// </summary>
	/// <param name="w">The weapon which is used</param>
	public delegate void Trigger(Weapon w);

	/// <summary>
	/// This event fires when the wepon is triggered/used
	/// </summary>
	public event Trigger TriggerEvent;


	public delegate void EmptyMagasin(Weapon w);

	public event EmptyMagasin EmptyMagasinEvent;

	public delegate void Reloaded(Weapon w);

	public event Reloaded ReloadedEvent;

	/// <summary>
	/// The parent object of this weapon, aka the player/owner of the weapon
	/// </summary>
	public GameObject parent;

	protected virtual void Start()
	{
		this.magasins = maxMagasins;
		this.bulletsInMagasin = maxBulletsInMagasin;
	}

	public override void Use()
	{
		if (Time.time - lastShootTime < shootTime) return;
		if (Time.time - lastReloadTime < reloadTime) return;


		if (!CheckBullets())
		{
			if(EmptyMagasinEvent != null) EmptyMagasinEvent.Invoke(this);
			if (autoReload)
				Reload();

			return;
		}
		if(TriggerEvent != null)
			TriggerEvent.Invoke(this);

		lastShootTime = Time.time;
	}

	public bool Reload()
	{
		if (Time.time - lastReloadTime < reloadTime) return false;
		if (maxMagasins <= 0) return false;
		bulletsInMagasin = maxBulletsInMagasin;
		magasins--;
		if(ReloadedEvent != null) ReloadedEvent.Invoke(this);
		lastReloadTime = Time.time;
		return true;
	}

	protected bool CheckBullets()
	{
		if (maxBulletsInMagasin <= 0) return true;
		if (bulletsInMagasin > 0) return true;
		return false;
	}



	public void Hitted(GameObject victem)
	{
		if(HitEvent != null)
			HitEvent.Invoke(victem, parent, this);
	}
}
