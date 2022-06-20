using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleScript : Unit
{
    public Animator animator;

    public void LightAttack()
    {
        StartCoroutine(Attack("Attack"));
    }

    public void HeavyAttack()
    {
        StartCoroutine(Attack("ChargedAttack"));
    }

    public void Hit()
    {
        StartCoroutine(Attack("Hit"));
    }

    private IEnumerator Attack(string triggerName)
    {
        animator.SetTrigger(triggerName);
        if (triggerName == "Attack")
        {
            currentStamina -= 10;
            yield return new WaitForSeconds(.667f);
        }
        else if (triggerName == "ChargedAttack")
        {
            currentStamina -= 20;
            yield return new WaitForSeconds(1.167f);
        }
        else
        {
            yield return new WaitForSeconds(.167f);
        }
    }
}
