using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : Tower
{
    [Space(10f)]
    [Header("Projectile Tower Fields")] //TODO: update to be Projectile Tower Fields and make ProjectileTower subclass instead of directly making it
    [SerializeField] protected GameObject _projectile;
    [SerializeField] protected float _projectileOffset;
    protected Vector2 _projectileSpawnOffset;
    protected Vector2 _projectileSpawnPos;

    public override void Awake()
    {
        base.Awake();
    }
    public override void Update()
    {
        base.Update();
        _projectileSpawnPos = (Vector2)transform.position + (_projectileSpawnOffset * transform.right);
    }
    public override void Attack()
    {
        if (NextAttackableShape == null) return;
        //spawn projectile
        //parent projectile to this gameobject
        //send towards the target shape
        Projectile projectile = Projectile.Instantiate(_projectile, _projectileSpawnPos, Quaternion.identity, this);
        projectile.name = $"{Properties._damageTypes} Projectile";
        projectile.Properties._target = NextAttackableShape.transform;
        projectile.Properties._dir = NextAttackableShape.transform.position - transform.position;
        projectile.Properties._distBeforeDespawn = Properties._projectileTravelDistance;
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        //_projectileSpawnOffset *= transform.right;
        _projectileSpawnOffset = new Vector2(_projectileOffset, _projectileOffset);
        _projectileSpawnPos = (Vector2)transform.position + (_projectileSpawnOffset * transform.right);
        if (Properties._isSelected)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(_projectileSpawnPos, 0.1f);
        }
    }
}
