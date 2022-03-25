using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemGenerator 
{
    public static ItemInterface[] GoodItems;
    public static ItemInterface Bomb;
    public static ItemInterface Obstacle;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        GoodItems = Resources.LoadAll<ItemInterface>("GoodItems/");
        Bomb = Resources.Load<ItemInterface>("BadItems/Bomb");
        Obstacle = Resources.Load<ItemInterface>("BadItems/Obstacle");
    }

    public static ItemInterface GenerateGoodItem()
    {
        return GoodItems[Random.Range(0, GoodItems.Length)];
    }

    public static ItemInterface MaybeGenerateABomb()
    {
        if (Random.Range(0, 100) < 20)
            return Bomb;
        return GenerateGoodItem();
    }
    public static ItemInterface MaybeGenerateObstacle()
    {
        if (Random.Range(0, 100) < 10)
            return Obstacle;
        return GenerateGoodItem();
    }

    public static ItemInterface GenerateForMatchCount(int matchCount)
    {
        switch (matchCount)
        {
            case 3:
                return GenerateGoodItem();

            case 4:
                return MaybeGenerateABomb();

            case 5:
                return MaybeGenerateObstacle();

            default:
                return GenerateGoodItem();
        }
    }
}
