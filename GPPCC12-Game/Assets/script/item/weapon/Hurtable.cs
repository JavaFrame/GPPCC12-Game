using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hurtable : MonoBehaviour
{
	/// <summary>
	/// the current life of the gameobject to which this component is attached to. 
	/// If a hurtable component is on a gameobject, then that means, that it can be hit by a weapon.
	/// </summary>
	[SerializeField]
	private int life;

	/// <summary>
	/// The maximum life this gameobject can have. It is also the init value of life
	/// </summary>
	[SerializeField]
	private int maxLife;

	/// <summary>
	/// An delegate which is used when the we
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="from"></param>
	/// <param name="weapon"></param>
	public delegate void Hitted(int damage, GameObject from, Weapon weapon);
	/// <summary>
	/// This event gets triggered if this gameobject got hit by a weapon
	/// </summary>
	public event Hitted HittedEventHandler;
	
	/// <summary>
	/// This delegate is used if this hurtable is killed
	/// </summary>
	/// <param name="victim">the victim which died</param>
	/// <param name="from">the attacker who killed the victim</param>
	/// <param name="weapon">the weapon which did the actual work</param>
	public delegate void Died(GameObject victim, GameObject from, Weapon weapon);

	/// <summary>
	/// This event gets fired if this hurtable dies
	/// </summary>
	public event Died DiedEventHandler;

	/// <summary>
	/// This function is called, if this hurtable was hit by a weapon
	/// </summary>
	/// <param name="damage"></param>
	/// <param name="weapon"></param>
	/// <param name="from"></param>
	public void Damaged(int damage, GameObject from, Weapon weapon)
	{
		this.life -= damage;
		if (this.life < 0)
		{
			this.life = 0;
			DiedEventHandler.Invoke(this.gameObject, from, weapon);
		}
		HittedEventHandler.Invoke(damage, from, weapon);
	}
}
