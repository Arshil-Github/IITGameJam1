using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Defense Upgrade", menuName = "New Defense Upgrade")]
public class DefenseUpgrades : ScriptableObject
{
    public string nameOfDefense;
    public int level;

    public int health;
    public int damage;
    public int attackDelay;
    public int range;

    public Sprite primarySprite;
    public Sprite secondarySprite;

}
