using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public static MainMenu Instance
	{
		get;
		private set;
	}

	[SerializeField]
	private GameObject mainPanelGo, joinMatchGo, createMatchGo;

	[SerializeField]
	private Animation mainPanelAnimation, joinMatchPanelAnimation;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		ShowMainPanel();
	}

	public void ShowJoinMatchPanel()
	{
		mainPanelGo.SetActive(false);
		createMatchGo.SetActive(false);
		joinMatchGo.SetActive(true);
	}

	public void ShowMainPanel()
	{
		joinMatchGo.SetActive(false);
		createMatchGo.SetActive(false);
		mainPanelGo.SetActive(true);
	}

	public void ShowCreateMatchPanel()
	{
		joinMatchGo.SetActive(false);
		mainPanelGo.SetActive(false);
		createMatchGo.SetActive(true);

	}


	private IEnumerator MoveTo(RectTransform trans, Vector3 to, Action callback = null, int steps = 100)
	{
		bool there = false;
		Vector3 diff = to -  trans.position;
		while (trans.position != to)
		{
			float delta = Time.deltaTime;
			trans.position += new Vector3(diff.x * delta, diff.y * delta, diff.z * delta);
			yield return new WaitForEndOfFrame();
		}
		if(callback != null)
			callback.Invoke();
	}
}
