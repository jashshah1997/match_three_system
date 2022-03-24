using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MatchThreeItem")]
public class ItemInterface : ScriptableObject
{
    public int scoreValue;
    public bool isObstacle;
    public bool isBomb;
    public Sprite itemSprite;
}
