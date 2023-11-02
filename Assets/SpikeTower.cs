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
    public override void Attack()
    {
        if (_specialUpgrades.Count <= 0)
        {
            base.Attack();
        }
        else if (_specialUpgrades.Contains(SpecialUpgrades.QuintupleShot))
        {
            Attack_QuintupleShot();
        }
        else if (_specialUpgrades.Contains(SpecialUpgrades.TripleShot))
        {
            Attack_TripleShot();
        }
    }
    private void Attack_QuintupleShot()
    {
        if (NextAttackableShape == null) return;
        float offset = 0.3f;
        for (int i = 0; i < 5; i++)
        {
            Projectile projectile = Projectile.Instantiate(_projectile, _projectileSpawnPos, Quaternion.identity, this);
            projectile.name = $"{Properties._damageTypes} Projectile";

            projectile.Properties._target = NextAttackableShape.transform;
            Vector3 addVector = Vector3.zero;
            if (i == 0)
            {
                addVector = new Vector3(offset, offset, 0);
            }
            else if (i == 1)
            {
                addVector = new Vector3(-offset, -offset, 0);
            }
            else if (i == 2)
            {
                addVector = new Vector3(-offset * 2, -offset * 2, 0);
            }
            else if (i == 3)
            {
                addVector = new Vector3(offset * 2, offset * 2, 0);
            }
            projectile.Properties._dir = (NextAttackableShape.transform.position + addVector) - transform.position;
            projectile.Properties._distBeforeDespawn = Properties._projectileTravelDistance;
        }
    }

    private void Attack_TripleShot()
    {
        if (NextAttackableShape == null) return;
        float offset = 0.6f;
        for (int i = 0; i < 3; i++)
        {
            Projectile projectile = Projectile.Instantiate(_projectile, _projectileSpawnPos, Quaternion.identity, this);
            projectile.name = $"{Properties._damageTypes} Projectile";

            projectile.Properties._target = NextAttackableShape.transform;
            Vector3 addVector = Vector3.zero;
            if (i == 0)
            {
                addVector = new Vector3(offset, offset, 0);
            }
            else if (i == 1)
            {
                addVector = new Vector3(-offset, -offset, 0);
            }
            projectile.Properties._dir = (NextAttackableShape.transform.position + addVector) - transform.position;
            projectile.Properties._distBeforeDespawn = Properties._projectileTravelDistance;
        }
    }
}

