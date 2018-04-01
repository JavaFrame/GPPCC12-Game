using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField]
    private DeathHurtable hurtable;

    [SerializeField]
    private Text lbHP;

    [SerializeField]
    private Image hpBar;

    private void Start()
    {
		if(hurtable == null)
			throw new Exception("Hurtable was null in HealthBar");
        lbHP.text = "HP: " + hurtable.life + " / " + hurtable.maxLife;
        hurtable.HittedEventHandler += (damage, from, weapon) =>
        {
	        hpBar.fillAmount = (float) hurtable.life / hurtable.maxLife;
	        lbHP.text = "HP: " + hurtable.life + " / " + hurtable.maxLife;
        };
    }
}
