using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{
    Item item = new Item { itemType = Item.ItemType.Coin, amount = 5 };

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerScript>())
        {
            PlayerScript player = collision.GetComponent<PlayerScript>();
            if (player.unitLevel > 5)
            {
                foreach (Item inventoryItem in player.inventory.GetItemList())
                {
                    if (inventoryItem.itemType == item.itemType)
                    {
                        if (inventoryItem.amount >= item.amount)
                        {
                            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
}
