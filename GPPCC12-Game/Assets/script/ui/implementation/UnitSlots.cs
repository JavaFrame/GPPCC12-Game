using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlots : MonoBehaviour
{
	[SerializeField] private string name;

	[SerializeField]
	private int ironCost;

	[SerializeField]
	private int oilCost;

	[SerializeField]
	private Spawner.SpawnerPrefab unit;

	[SerializeField]
	private Text titleLabel;

	[SerializeField]
	private Text ironCostLbl;

	[SerializeField]
	private Text oilCostLbl;
	// Use this for initialization
	void Start ()
	{
		titleLabel.text = name;
		ironCostLbl.text = ironCost + " Iron";
		oilCostLbl.text = oilCost + " Oil";
	}

	public void TrainBtnListener()
	{
		//TODO check if enough resources are avaible and use them
		Spawner.SpawnerInstance.CmdSpawnUnit((int) unit);
	}

}
