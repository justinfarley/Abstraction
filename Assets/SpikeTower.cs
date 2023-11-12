using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTower : ProjectileTower
{
    [SerializeField] private GameObject _spikeBall, _ultraSpikeBall;
    public override void Awake()
    {
        base.Awake();
        Init_Tower(1.5f, 2, 1,
    new List<DamageTypes> { DamageTypes.Spikes }, 2, TowerProperties.Targeting.First,
    _projectile, 3f, 3f);
        _projectileSpawnOffset = new Vector2(_projectileOffset, _projectileOffset);
        Name = "SpikeTower";
        UpdateRadiusGraphics();
    }
    public override void Attack()
    {
        if (NextAttackableShape == null) return;
        if (_upgrades[0] == 0 && _upgrades[1] == 0 && _upgrades[2] == 0)
        {
            base.Attack();
            return;
        }
        if (_upgrades[0]  > 0)
        {
            HandleTopPathAttack();
        }
        if (_upgrades[1] > 0)
        {
            HandleMiddlePathAttack();
        }
        if (_upgrades[2] > 0)
        {
            HandleBottomPathAttack();
        }
    }
    private void HandleTopPathAttack()
    {
        if (_specialUpgrades.Count <= 0)
        {
            base.Attack();
        }
        else if (_specialUpgrades.Contains(SpecialUpgrades.UltraSpikeBall))
        {
            Attack_UltraSpikeBall();
        }
        else if (_specialUpgrades.Contains(SpecialUpgrades.SpikeBall))
        {
            Attack_SpikeBall();
        }
    }
    private void HandleMiddlePathAttack()
    {
        base.Attack();
    }
    private void HandleBottomPathAttack()
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

    private void Attack_UltraSpikeBall()
    {
        Projectile.Instantiate(_ultraSpikeBall, _projectileSpawnPos, Quaternion.identity, this);
    }

    private void Attack_SpikeBall()
    {
        Projectile.Instantiate(_spikeBall, _projectileSpawnPos, Quaternion.identity, this);
    }

    private void Attack_QuintupleShot()
    {
        float offset = 0.3f;
        for (int i = 0; i < 5; i++)
        {
            Projectile projectile = Projectile.Instantiate(_projectile, _projectileSpawnPos, Quaternion.identity, this);
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
        }
    }

    private void Attack_TripleShot()
    {
        float offset = 0.6f;
        for (int i = 0; i < 3; i++)
        {
            Projectile projectile = Projectile.Instantiate(_projectile, _projectileSpawnPos, Quaternion.identity, this);
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
        }
    }
}

