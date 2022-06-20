using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemWorldSpawner : MonoBehaviour
{
    public static ItemWorldSpawner I_IWS { get; private set; }

    public Item coin;
    public Item health;
    public Item stamina;

    public GameObject enemy;
    public GameObject player;
    public GameObject playerCamera;
    public GameObject exit;

    public Tilemap floor;

    private int exitCount;
    private int playerCount;

    private void Awake()
    {
        I_IWS = this;
    }
    
    public void SpawnItems()
    {
        for (var x = floor.cellBounds.min.x; x < floor.cellBounds.max.x; x++)
        {
            for (var y = floor.cellBounds.min.x; y < floor.cellBounds.max.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (floor.HasTile(pos))
                {
                    if (Random.value > 0.94)
                    {
                        ItemWorld.SpawnItemWorld(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z), coin);
                    }
                    if (Random.value > 0.995)
                    {
                        ItemWorld.SpawnItemWorld(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z), health);
                    }
                    if (Random.value > 0.9875)
                    {
                        ItemWorld.SpawnItemWorld(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z), stamina);
                    }
                    if (Random.value > 0.99)
                    {
                        var enemyVar = enemy.GetComponent<EnemyScript>();
                        enemyVar.unitLevel = Random.Range(1, 10);
                        enemyVar.XP = Random.Range(1, 10) + enemyVar.unitLevel;
                        enemyVar.coins = Random.Range(5, 20);
                        
                        Instantiate(enemy, new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z), Quaternion.identity);
                    }
                    if (Random.value > 0.5 && playerCount == 0)
                    {
                        Instantiate(playerCamera, new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z), Quaternion.identity);
                        CameraFollow.I_CF.target = Instantiate(player, new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z), Quaternion.identity).transform;
                        playerCount = 1;
                    }
                    if (Random.value > 0.9 && exitCount < 5)
                    {
                        Instantiate(exit, new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z), Quaternion.identity);
                        exitCount += 1;
                    }
                }
            }
        }
        Destroy(gameObject);
    }
}
 