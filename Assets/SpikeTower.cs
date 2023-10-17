using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTower : ProjectileTower
{
    public override void Awake()
    {
        base.Awake();

        Init_Tower(1, 2, 1,
    DamageTypes.Spikes, 1, TowerProperties.AttackType.First,
    _projectile, 3f);
        _projectileSpawnOffset = new Vector2(_projectileOffset, _projectileOffset);
    }
}
