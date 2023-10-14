using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity, IDamageable
{
    [Space(10f)]
    [Header("Projectile Fields")]
    public ProjectileProperties projectileProperties = new();
    public float DamageGiven { get; set; }
    public float DamageTaken { get; set; }
    /// <summary>
    /// Damages an entity
    /// </summary>
    /// <param name="damageableEntity">The entity getting damaged</param>
    /// <param name="dmg">amount of damage to apply</param>
    public void DealDamage(IDamageable damageableEntity, float dmg)
    {
        if(!IsDamageable(transform.parent.GetComponent<Entity>()))
        {
            Debug.LogError("PROJECTILE " + gameObject.name + " HAS NO DAMAGEABLE PARENT");
            return;
        }
        //since all projectiles should be instantiated as a child of the thing shooting them
        damageableEntity.TakeDamage(transform.parent.GetComponent<IDamageable>(), dmg);
    }

    public void TakeDamage(IDamageable damageDealer, float dmg)
    {
        //cant take damage, can only give (since its a projectile)
        return;
    }
    private void Awake()
    {
        CurrentState = State.Invulnerable; //as a projectile, it is invulnerable from the start until it disappears (or dies)
                                           //if certain projectiles are going to be killable in the future then change this code.
    }
    [Serializable]
    public class ProjectileProperties
    {
        public float Damage { get => damage; set => damage = value; }
        
        [SerializeField] private float damage;
    }
}
