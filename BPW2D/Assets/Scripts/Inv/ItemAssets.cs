using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets I_ItemAssets { get; private set; }

    private void Awake()
    {
        I_ItemAssets = this;
    }

    public Transform pfItemWorld;

    public Sprite healthPoitionSprite;
    public Sprite manaPotionSprite;
    public Sprite coinSprite;
}
