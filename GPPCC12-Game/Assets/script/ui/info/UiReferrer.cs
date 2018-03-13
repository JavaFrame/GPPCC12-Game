using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiReferrer : MonoBehaviour
{
	public GameObject canvasGo;

    public enum StructureType { Base, OilMine, IronMine, HealingUnit, DpsUnit, TankUnit };

    public StructureType type;
}
