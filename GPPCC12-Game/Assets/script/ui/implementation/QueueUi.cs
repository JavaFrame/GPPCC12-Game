using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueUi : MonoBehaviour
{
	public static QueueUi Instance
	{
		get;
		private set;
	}

	[SerializeField] private Text spawningText;
	[SerializeField] private Text spaningTimeText;

	[SerializeField] private Image[] images;

	[SerializeField] private List<UnitSlot.UnitSlotData> slots = new List<UnitSlot.UnitSlotData>();


	private bool currentlySpawing = false;

	void Start ()
	{
		if(QueueUi.Instance != null)
			throw new Exception("There are multiple QueueUi instances in the scene!");
		QueueUi.Instance = this;
	}

	public void UpdateQueue()
	{
		for (int i = 0; i < images.Length; i++)
			images[i].sprite = null;
		for(int i = 0; i < slots.Count; i++)
			images[i].sprite = slots[i].UnitSprite;
		

		if (!currentlySpawing && slots.Count > 0)
			StartCoroutine(SpawnAfterSpawnTime(slots[0]));
	}

	public bool Add(UnitSlot.UnitSlotData data)
	{
		if (slots.Count >= images.Length) return false;
		if (Inventory.IronResource < data.IronCost) return false;
		if (Inventory.OilResource < data.OilCost) return false;
		Inventory.IronResource -= data.IronCost;
		Inventory.OilResource -= data.OilCost;
		slots.Add(data);
		UpdateQueue();
		return true;
	}

	public IEnumerator SpawnAfterSpawnTime(UnitSlot.UnitSlotData data)
	{
		spawningText.text = data.Name;
		spaningTimeText.text = data.SpawnTime + " sec";
		currentlySpawing = true;
		float startTime = Time.time;
		yield return new WaitWhile(() =>
		{
			spaningTimeText.text = data.SpawnTime - (Time.time - startTime) + " sec";
			return Time.time - startTime < data.SpawnTime;
		});
		Spawner.SpawnerInstance.CmdSpawnUnit((int) data.Unit);
		slots.RemoveAt(0);
		for (int i = slots.Count - 1; i >= slots.Count; i--)
		{ 
			slots[i] = slots[i + 1];
		}

		currentlySpawing = false;
		UpdateQueue();
		spawningText.text = "-";
		spaningTimeText.text = "- sec";
	}

}
