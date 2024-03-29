using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    public string Name;
    public string Description;
    public int Price;
    public int PierceIncrease;
    public float DamageIncrease;
    public float AttackDelayDecrease;
    public float RangeIncrease;
    public float ProjectileSpeedIncrease;
    public float ProjectileTravelDistanceIncrease;
    public List<DamageTypes> DamageTypesToAdd;
    public List<Debuffs> DebuffsToAdd;
    public bool canHitCamo = false;
    public Sprite upgradeSprite;
    public SpecialUpgrades SpecialUpgrade = SpecialUpgrades.None;
}
