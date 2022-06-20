using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler I_GH { get; set; }
    private static GameHandler gameHandler;
    private GameObject dungeonGenerator;

    private void Awake()
    {
        I_GH = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (gameHandler == null)
        {
            gameHandler = this;

            dungeonGenerator = GameObject.Find("RoomsFirstDungeonGenerator");
            dungeonGenerator.GetComponentInChildren<AbstractDungeonGenerator>().GenerateDungeon();
            ItemWorldSpawner.I_IWS.SpawnItems();

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
