using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyScript : Unit
{
    public static EnemyScript I_ES;

    private bool isMoving;
    public bool inBattle = false;

    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Vector3 origPos, targetPos;
    public float timeToMove = 5f;
    private int timeBetweenMove = 0;
    private int maxTimeBetweenMove = 400;
    
    public TMP_Text levelText;
    
    private void Awake()
    {
        I_ES = this;
        maxTimeBetweenMove += Random.Range(50, 250);
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        levelText.text = "Level " + unitLevel;
    }
    private void Update()
    {
        if (timeBetweenMove < maxTimeBetweenMove)
        {
            timeBetweenMove += 1;
        }
        else
        {
            timeBetweenMove = 0;
            if (!inBattle)
            {
                EnemyWalking();
            }
        }
    }

    private void EnemyWalking()
    {
        float chance = Random.value;
        if (chance < .25f && !isMoving && !CheckTiles(Vector3.up))
        {
            animator.SetTrigger("Walk");
            StartCoroutine(MoveEnemy(Vector3.up));
        }
        if (chance > .25f && chance < .50f && !isMoving && !CheckTiles(Vector3.left))
        {
            animator.SetTrigger("Walk");
            StartCoroutine(MoveEnemy(Vector3.left));
            spriteRenderer.flipX = true;

        }
        if (chance > .50f && chance < .75f && !isMoving && !CheckTiles(Vector3.down))
        {
            animator.SetTrigger("Walk");
            StartCoroutine(MoveEnemy(Vector3.down));
        }
        if (chance > .75f && !isMoving && !CheckTiles(Vector3.right))
        {
            animator.SetTrigger("Walk");
            StartCoroutine(MoveEnemy(Vector3.right));
            spriteRenderer.flipX = false;
        }
    }

    private bool CheckTiles(Vector3 direction)
    {
        origPos = transform.position;
        targetPos = origPos + direction;
        if (TilemapVisualizer.instance)
        {
            return TilemapVisualizer.instance.pWall.HasTile(Vector3Int.FloorToInt(targetPos));
        }
        else
        {
            return true;
        }
    }

    private IEnumerator MoveEnemy(Vector3 direction)
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

    public void Attack()
    {
        StartCoroutine(Attack("Attack"));
    }

    private IEnumerator Attack(string triggerName)
    {
        animator.SetTrigger(triggerName);

        yield return new WaitForSeconds(.667f);
    }
}
