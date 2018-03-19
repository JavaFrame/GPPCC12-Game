using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : Hurtable {

    [SerializeField]
    private DeathHurtable hurtable;

    [SerializeField]
    private Text lbHP;

    [SerializeField]
    private Image hpBar;

    private void Start()
    {
        lbHP.text = "HP: " + hurtable.life + " / " + hurtable.maxLife;
        hurtable.HittedEventHandler += (damage, from, weapon) => hpBar.fillAmount = (float) hurtable.life / hurtable.maxLife;
    }
}
