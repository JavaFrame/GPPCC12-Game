using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour {
	public static GameObject CoreGO
	{
		get;
		private set;
	}

	public static Core Instance
	{
		get;
		private set;
	}

	[SerializeField]
	private float matchTime;

	private float startTime;

	public float MatchTime
	{
		get { return matchTime; }
	}

	public float StartTime
	{
		get { return startTime; }
	}

	void Awake () {
		if(CoreGO != null)
			throw new Exception("There are multiple Cores in the scene");
		CoreGO = this.gameObject;
		Instance = this; 
	}

	void Start()
	{
		Hurtable h = GetComponent<Hurtable>();
		h.DiedEventHandler += (victim, from, weapon) => GameStateManager.Instance.FpsPlayersWon();
		StartCoroutine(EndMatchTimeout());
	}

	IEnumerator EndMatchTimeout()
	{
		startTime = Time.time;
		yield return new WaitForSeconds(matchTime);
		GameStateManager.Instance.RtsPlayerWon();
	}
}
