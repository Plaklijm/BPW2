using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int coins;
    public int XP;

    public int lightDamage = 7;
    public int heavyDamage = 20;

    public int maxHP = 100;
    public int currentHP;
    public int maxStamina = 100;
    public int currentStamina;

    private void awake()
    {
        lightDamage += unitLevel * 2;
        heavyDamage += unitLevel;
        coins = Random.Range(5, 20);

        currentHP = maxHP;
        currentStamina = maxStamina;
    }

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public bool Heal(int heal)
    {
        currentHP += heal;
        if (currentHP >= maxHP)
            return true;
        else
            return false;
    }

    public bool Stamina(int stamina)
    {
        currentStamina += stamina;
        if (currentStamina >= maxStamina)
            return true;
        else
            return false;
    }
}
