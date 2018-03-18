using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultInfoUi : UiParent
{
	[SerializeField]
	private Description description;

	[SerializeField]
	private Text titleText;
	[SerializeField]
	private Text descriptionText;

	void Start ()
	{
		if (description == null)
		{
			description = Parent.GetComponent<Description>();
		}

		if (description == null)
		{
			Debug.LogWarning("Description was null");
			return;
		}
		titleText.text = description.ObjectTitle;
		descriptionText.text = description.ObjectDescription;
	}
}
