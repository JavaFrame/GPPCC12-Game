using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthInformations : MonoBehaviour
{
	private Hurtable baseHurtable, oilHurtable, iron1Hurtable, iron2Hurtable;

	[SerializeField]
	private Text baseText, oilText, iron1Text, iron2Text;

	void Start()
	{
		baseHurtable = Core.CoreGO.GetComponent<Hurtable>();
		var resources = Resources.ResourcesDictionary;
		var oilGo = resources[ResourceGenerator.Resource.Oil][0];
		var iron1Go = resources[ResourceGenerator.Resource.Iron][0];
		var iron2Go = resources[ResourceGenerator.Resource.Iron][1];
		Debug.Log(resources);

		oilHurtable = oilGo.GetComponent<Hurtable>();
		iron1Hurtable = iron1Go.GetComponent<Hurtable>();
		iron2Hurtable = iron2Go.GetComponent<Hurtable>();

		baseText.text = baseHurtable.life + "/" + baseHurtable.maxLife;
		oilText.text = oilHurtable.life + "/" + oilHurtable.maxLife;
		iron1Text.text = iron1Hurtable.life + "/" + iron1Hurtable.maxLife;
		iron2Text.text = iron2Hurtable.life + "/" + iron2Hurtable.maxLife;

		baseHurtable.HittedEventHandler += (damage, from, weapon) => baseText.text = baseHurtable.life + "/" + baseHurtable.maxLife;
		oilHurtable.HittedEventHandler += (damage, from, weapon) => oilText.text = oilHurtable.life + "/" + oilHurtable.maxLife;
		iron1Hurtable.HittedEventHandler += (damage, from, weapon) => iron1Text.text = iron1Hurtable.life + "/" + iron1Hurtable.maxLife;
		iron2Hurtable.HittedEventHandler += (damage, from, weapon) => iron2Text.text = iron2Hurtable.life + "/" + iron2Hurtable.maxLife;
	}
}
