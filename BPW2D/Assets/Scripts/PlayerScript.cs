using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : Unit
{
    public static PlayerScript I_PlayerScript { get; set; }
    private bool isMoving;
    private bool attacking = false;
    public bool inBattle = false;

    private Vector3 origPos, targetPos;
    public float timeToMove = .5f;

    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public GameObject miniMap;

    public Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;
    public GameObject InventoryUI;

    public int experience;
    private int experienceToNextLevel = 25;
    public Slider xpBar;
    public TMP_Text levelText;

    private void Awake()
    {
        I_PlayerScript = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        inventory = new Inventory(UseItem);
        uiInventory.SetPlayer(this);
        uiInventory.SetInventory(inventory);
        levelText.text = "Level " + 1;
    }

    private void Update()
    {
        if (!inBattle)
        {
            PlayerInput();
        }
    }

    public void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPoition:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPoition, amount = 1 });
                break;
            case Item.ItemType.ManaPotion:
                inventory.RemoveItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });
                break;
        }
    }

    private void PlayerInput()
    {
        if (Input.GetKey(KeyCode.W) && !isMoving && !CheckTiles(Vector3.up) && !attacking)
        {
            animator.SetTrigger("Run");
            StartCoroutine(MovePlayer(Vector3.up));
        }
        if (Input.GetKey(KeyCode.A) && !isMoving && !CheckTiles(Vector3.left) && !attacking)
        {
            animator.SetTrigger("Run");
            StartCoroutine(MovePlayer(Vector3.left));
            spriteRenderer.flipX = true;

        }
        if (Input.GetKey(KeyCode.S) && !isMoving && !CheckTiles(Vector3.down) && !attacking)
        {
            animator.SetTrigger("Run");
            StartCoroutine(MovePlayer(Vector3.down));
        }
        if (Input.GetKey(KeyCode.D) && !isMoving && !CheckTiles(Vector3.right) && !attacking)
        {
            animator.SetTrigger("Run");
            StartCoroutine(MovePlayer(Vector3.right));
            spriteRenderer.flipX = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
        else if (collision.gameObject.GetComponent<EnemyScript>())
        {
            Destroy(collision.gameObject);
            SceneManager.UnloadSceneAsync("SampleScene");
            SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Single);
        }
    }
    
    private bool CheckTiles(Vector3 direction)
    {
        origPos = transform.position;
        targetPos = origPos + direction;

        return TilemapVisualizer.instance.pWall.HasTile(Vector3Int.FloorToInt(targetPos));
    }
    
    public void LightAttack()
    {
        StartCoroutine(Attack("Attack"));
    }

    public void HeavyAttack()
    {
        StartCoroutine(Attack("ChargedAttack"));
    }

    private IEnumerator Attack(string triggerName)
    {
        attacking = true;
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

        attacking = false;
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }

    public void AddXP(int XPToAdd)
    {
        experience += XPToAdd;
        xpBar.maxValue = experienceToNextLevel;
        xpBar.value = experience;
        if (experience >= experienceToNextLevel)
        {
            unitLevel += 1;
            experience -= experienceToNextLevel;
            xpBar.value = experience;
            levelText.text = "Level " + unitLevel;
            maxHP += 10;
            maxStamina += 10;
            lightDamage += 2;
            heavyDamage += 3;
        }
    }

    public void AddCoins(int coinsToAdd)
    {
        inventory.AddItem(new Item { itemType = Item.ItemType.Coin, amount = coinsToAdd });
    }
}
