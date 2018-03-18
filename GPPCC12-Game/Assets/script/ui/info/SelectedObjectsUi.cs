using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SelectedObjectsUi : MonoBehaviour
{
	[SerializeField] private Text selectedText;
	[SerializeField] private Text secundarySelectedText;
	// Use this for initialization
	void Start ()
	{
		InfoShower.Instance.ChangeSelectionEvent += (added, removed, selected) => selectedText.text = "Primary: " + GameObjectListToString(selected);
		InfoShower.Instance.ChangeSecundarySelectionEvent += (added, removed, selected) => secundarySelectedText.text = "Secundary: " + GameObjectListToString(selected);
	}

	private string GameObjectListToString(ReadOnlyCollection<GameObject> gos)
	{
		List<string> result = new List<string>();
		foreach (var go in gos)
		{
			Description d = go.GetComponent<Description>();
			if (d != null)
				result.Add(d.ObjectTitle);
			else
				result.Add(go.name);
		}

		return String.Join(", ", result.ToArray());
	}

}
