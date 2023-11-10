using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity, IDamageable
{
    [Space(10f)]
    [Header("Projectile Fields")]
    public ProjectileProperties Properties;
    private Tower.TowerProperties _towerProperties;
    protected int _pierce;
    protected float _speed;
    private Vector2 spawnPos;
    public float DamageGiven { get; set; }
    public float DamageTaken { get; set; }

    public bool isDirect = true;
    public int Pierce { get => _pierce; set => _pierce = value; }
    public Transform Target { get => Properties._target; set => Properties._target = value; }
    /// <summary>
    /// Damages an entity
    /// </summary>
    /// <param name="damageableEntity">The entity getting damaged</param>
    /// <param name="dmg">amount of damage to apply</param>
    public void DealDamage(IDamageable damageableEntity, float dmg)
    {
        if(!IsDamageable(Properties._sourceParent))
        {
            Debug.LogError("PROJECTILE " + gameObject.name + " HAS NO DAMAGEABLE SOURCE PARENT");
            return;
        }
        //since all projectiles should be instantiated as a child of the thing shooting them
        damageableEntity.TakeDamage(Properties._sourceParent, dmg);
    }
    public virtual void DealDamage(AbstractShapeEnemy damageableEntity, List<DamageTypes> damageTypes)
    {
        damageableEntity.TakeDamage(Properties._sourceParent, _towerProperties._attackDamage, damageTypes);
        _pierce--;
        if (_pierce <= 0) Destroy(gameObject);
    }
    public static Projectile Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Tower sourceParent)
    {
        GameObject go = Instantiate(prefab, position, rotation);
        Projectile p = go.GetComponent<Projectile>();
        p.Properties._sourceParent = sourceParent;
        p._speed = sourceParent.Properties._projectileSpeed;
        p.name = $"Projectile";
        p.Properties._target = sourceParent.NextAttackableShape != null ? sourceParent.NextAttackableShape.transform : null;
        p.Properties._dir = sourceParent.NextAttackableShape != null ? ((sourceParent.NextAttackableShape.transform.position) - sourceParent.transform.position) : Vector2.zero;
        p.Properties._distBeforeDespawn = sourceParent.Properties._projectileTravelDistance;
        p._pierce = sourceParent.Properties._projectilePierce;
        return p;
    }
    public void TakeDamage(IDamageable damageDealer, float dmg)
    {
        //cant take damage, can only give (since its a projectile)
        return;
    }
    private void Awake()
    {
        EntityType = Type.Projectile;
        CurrentState = State.Invulnerable; //as a projectile, it is invulnerable from the start until it disappears (or dies)
                                           //if certain projectiles are going to be killable in the future, change this code.
    }
    private void Start()
    {
        spawnPos = transform.position;
        _towerProperties = Properties._sourceParent.Properties;
        if (Properties._sourceParent.NextAttackableShape != null && isDirect)
        {
            transform.right = Properties._sourceParent.NextAttackableShape.transform.position - transform.position;
        }
    }
    private void Update()
    {
        if (Properties._dir == null) return;
        EntityRigidbody.velocity = _speed * Properties._dir;
        if(Vector2.Distance(spawnPos, transform.position) >= Properties._distBeforeDespawn)
        {
            Destroy(gameObject);
        }
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<AbstractShapeEnemy>())
        {
            AbstractShapeEnemy enemy = collision.gameObject.GetComponent<AbstractShapeEnemy>();
            print($"Dealing damage to {enemy.name}");
            Properties._sourceParent.AddTowerDebuffsToShape(enemy);
            if(_pierce >= 1)
                DealDamage(enemy, _towerProperties._damageTypes);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    [Serializable]
    public struct ProjectileProperties
    {
        [SerializeField] internal Transform _target;
        internal Tower _sourceParent;
        internal Vector2 _dir;
        internal float _distBeforeDespawn;

    }
}
