using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Description
{
	
	/// <summary>
	/// The itemName of the item
	/// </summary>
	[SerializeField]
	private string itemName;

	/// <summary>
	/// the description of the item
	/// </summary>
	[SerializeField]
	private string description;

	/// <summary>
	/// Gets called when the item is used
	/// </summary>
	public abstract void Use();
}
