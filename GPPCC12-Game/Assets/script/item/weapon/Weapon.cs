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
	private int damage;

	public int Damage
	{
		get { return damage; }
		set { damage = value; }
	}

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

	/// <summary>
	/// The parent object of this weapon, aka the player/owner of the weapon
	/// </summary>
	public GameObject parent;


	public override void Use()
	{
		TriggerEvent.Invoke(this);
	}


	public void Hitted(GameObject victem)
	{
		HitEvent.Invoke(victem, parent, this);
	}
}
