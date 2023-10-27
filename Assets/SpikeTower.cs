using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTower : ProjectileTower
{
    public override void Awake()
    {
        base.Awake();
        Init_Tower(1f, 3, 1,
    new List<DamageTypes> { DamageTypes.Spikes }, 2, TowerProperties.Targeting.First,
    _projectile, 5f, 4.5f);
        _projectileSpawnOffset = new Vector2(_projectileOffset, _projectileOffset);
        Name = "SpikeTower";
    }
}
