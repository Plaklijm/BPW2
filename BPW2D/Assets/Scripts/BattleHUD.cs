using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text levelText;

    public Slider hpSlider;
    public Slider staminaSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        staminaSlider.maxValue = unit.maxStamina;
        staminaSlider.value = unit.currentStamina;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void SetStamina(int stamina)
    {
        staminaSlider.value = stamina;
    }
}
