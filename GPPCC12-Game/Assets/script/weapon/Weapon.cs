using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	[SerializeField]
	private int damage;

	public abstract void Trigger();

	public int Damage
	{
		get { return damage; }
		set { damage = value; }
	}
}
