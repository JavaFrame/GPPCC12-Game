using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitSlot : MonoBehaviour
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

	[SerializeField]
	private Image image;
	void Start ()
	{
		if(data == null)
			throw new Exception("data in UnitSlot is null");
		titleLabel.text = name;
		ironCostLbl.text = data.IronCost + " Iron";
		oilCostLbl.text = data.OilCost + " Oil";
        spawnTimeLbl.text = data.SpawnTime + " s";

		image.sprite = data.UnitSprite;
	}

	public void TrainBtnListener()
	{
		QueueUi.Instance.Add(data);
		//Spawner.SpawnerInstance.CmdSpawnUnit((int) data.Unit);
	}

	[Serializable]
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

	    [SerializeField]
	    private Texture2D _unitTexture;

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

	    public Texture2D UnitTexture
	    {
		    get { return _unitTexture; }
	    }

	    public Sprite UnitSprite
	    {
		    get
		    {
				if(UnitTexture == null)
					throw new Exception("Unit Texture is null!");
				return Sprite.Create(UnitTexture, new Rect(0, 0, UnitTexture.width, UnitTexture.height), Vector2.zero);
			}
	    }
    }
}
