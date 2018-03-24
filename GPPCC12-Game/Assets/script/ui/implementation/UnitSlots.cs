using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlots : MonoBehaviour
{
    [SerializeField]
    private UnitSlotData data;

	[SerializeField]
	private Text titleLabel;

	[SerializeField]
	private Text ironCostLbl;

	[SerializeField]
	private Text oilCostLbl;

    [SerializeField]
    private Text spawnTimeLbl;

	// Use this for initialization
	void Start ()
	{
		titleLabel.text = name;
		ironCostLbl.text = ironCost + " Iron";
		oilCostLbl.text = oilCost + " Oil";
        spawnTimeLbl.text = spawnTime + " s";
	}

	public void TrainBtnListener()
	{
		//TODO check if enough resources are avaible and use them
		Spawner.SpawnerInstance.CmdSpawnUnit((int) unit);
	}

    public class UnitSlotData
    {
        [SerializeField]
        private Spawner.SpawnerPrefab unit;

        [SerializeField]
        private string name;

        [SerializeField]
        private int ironCost;

        [SerializeField]
        private int oilCost;

        [SerializeField]
        private float spawnTime;

        public Spawner.SpawnerPrefab Unit
        {
            get { return unit;}
        }

        public string Name
        {
            get { return name; }
        }

        public int IronCost
        {
            get { return ironCost; }
        }

        public int OilCost
        {
            get { return oilCost; }
        }

        public float SpawnTime
        {
            get { return spawnTime; }
        }
    }
}
