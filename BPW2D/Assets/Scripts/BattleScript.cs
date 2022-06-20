using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleScript : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject playerPrefab;

    public Transform playerPlace;
    public Transform enemyPlace;

    private Unit playerUnit;
    private Unit enemyUnit;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    public static BattleScript BSInstance;

    Item health = new Item { itemType = Item.ItemType.HealthPoition, amount = 1};
    Item stamina = new Item { itemType = Item.ItemType.ManaPotion, amount = 1};

    public void Awake()
    {
        if (BSInstance != null)
        {
            Debug.Log("More than one BSInstance in scene");
        }
        BSInstance = this;
}

    public void Start()
    {
        StartBattle();
    }
    public void StartBattle()
    {
       state = BattleState.START;
       StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        GameObject playerObject = Instantiate(playerPrefab, playerPlace);
        playerUnit = playerObject.GetComponent<Unit>();

        PlayerScript.I_PlayerScript.inBattle = true;
        PlayerScript.I_PlayerScript.miniMap.SetActive(false);

        GameObject enemyObject = Instantiate(enemyPrefab, enemyPlace);
        enemyUnit = enemyObject.GetComponent<Unit>();

        EnemyScript.I_ES.inBattle = true;

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTURN;
    }

    public void OnLightAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        else if (playerUnit.currentStamina < 10)
            return;

        StartCoroutine(PlayerAttackLight());
    }

    public void OnHeavyAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        else if (playerUnit.currentStamina < 20)
            return;     

        StartCoroutine(PlayerAttackHeavy());
    }

    public void HealPlayer()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        foreach (Item inventoryItem in PlayerScript.I_PlayerScript.inventory.GetItemList())
        {
            if (inventoryItem.itemType == health.itemType)
            {
                if (inventoryItem.amount >= health.amount)
                {
                    PlayerScript.I_PlayerScript.UseItem(health);
                    StartCoroutine(PlayerHeal());
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    public void RecoverStamina()
    {

        if (state != BattleState.PLAYERTURN)
            return;

        foreach (Item inventoryItem in PlayerScript.I_PlayerScript.inventory.GetItemList())
        {
            if (inventoryItem.itemType == stamina.itemType)
            {
                if (inventoryItem.amount >= stamina.amount)
                {
                    PlayerScript.I_PlayerScript.UseItem(stamina);
                    StartCoroutine(PlayerStamina());
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    public void ExitBattle()
    {
        StartCoroutine(EndBattle());
    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            yield return new WaitForSeconds(2f);
            PlayerScript.I_PlayerScript.inBattle = false;
            PlayerScript.I_PlayerScript.miniMap.SetActive(true);
            PlayerScript.I_PlayerScript.AddXP(enemyUnit.XP);
            PlayerScript.I_PlayerScript.AddCoins(enemyUnit.coins);
            EnemyScript.I_ES.inBattle = true;
            SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
        }
        else if (state == BattleState.LOST)
        {
            yield return new WaitForSeconds(2f);
            PlayerScript.I_PlayerScript.inBattle = false;
            PlayerScript.I_PlayerScript.miniMap.SetActive(true);
            PlayerScript.I_PlayerScript.AddXP(enemyUnit.XP/2);
            PlayerScript.I_PlayerScript.AddCoins(enemyUnit.coins/4);
            EnemyScript.I_ES.inBattle = true;
            SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            PlayerScript.I_PlayerScript.inBattle = false;
            PlayerScript.I_PlayerScript.miniMap.SetActive(true);
            EnemyScript.I_ES.inBattle = true;
            SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
        }
    }

    IEnumerator PlayerAttackHeavy()
    {
        playerUnit.GetComponent<PlayerBattleScript>().HeavyAttack();
        bool isDead = enemyUnit.TakeDamage(playerUnit.heavyDamage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        playerHUD.SetStamina(playerUnit.currentStamina);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerAttackLight()
    {
        playerUnit.GetComponent<PlayerBattleScript>().LightAttack();
        bool isDead = enemyUnit.TakeDamage(playerUnit.lightDamage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        playerHUD.SetStamina(playerUnit.currentStamina);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        PlayerScript.I_PlayerScript.UseItem(new Item { itemType = Item.ItemType.HealthPoition, amount = 1 });
        playerUnit.Heal(10 + (playerUnit.unitLevel * 2));
        playerHUD.SetHP(playerUnit.currentHP);

        state = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(2f);
        StartCoroutine(EnemyTurn());

    }

    IEnumerator PlayerStamina()
    {
        PlayerScript.I_PlayerScript.UseItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });

        playerUnit.Stamina(10 + (playerUnit.unitLevel * 2));
        playerHUD.SetStamina(playerUnit.currentStamina);

        state = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(2f);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.lightDamage);
        enemyUnit.GetComponent<EnemyScript>().Attack();
        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
        }
    }
}
