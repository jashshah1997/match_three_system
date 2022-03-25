using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemGenerator 
{
    public static ItemInterface[] GoodItems;
    public static ItemInterface[] BadItems;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        GoodItems = Resources.LoadAll<ItemInterface>("GoodItems/");
        BadItems = Resources.LoadAll<ItemInterface>("BadItems/");
    }

    public static ItemInterface GenerateGoodItem()
    {
        return GoodItems[Random.Range(0, GoodItems.Length)];
    }
    public static ItemInterface GenerateBadItem()
    {
        return BadItems[Random.Range(0, BadItems.Length)];
    }
}
