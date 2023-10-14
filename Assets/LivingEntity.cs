using System;
using UnityEngine;

public abstract class LivingEntity : Entity, IDamageable
{
    public enum Hostility
    {
        Passive,
        Hostile,
    }
    public Hostility EntityHostility { get; set; }
    public float MaxHealth { get; set; }
    public float Health { get; set; }
    public float DamageGiven { get => damageGiven; set => damageGiven = value; }

    private float damageGiven;
    public float DamageTaken { get => damageTaken; set => damageTaken = value; }

    private float damageTaken;

    public Action OnDeath;


    public virtual void Awake()
    {
        Init_LivingEntity();
    }
    private void Init_LivingEntity()
    {
        OnDeath = null;
        Health = MaxHealth;
        CurrentState = State.Alive;
        OnDeath += () => CurrentState = State.Dead;
    }
    public virtual void TakeDamage(IDamageable damageDealer, float dmg)
    {
        if (!IsDamageable(this) || dmg < 0) return;
        if (CurrentState == State.Invulnerable) return;
        Health -= dmg;
        if(damageDealer != null)
            damageDealer.DamageGiven += dmg;
        DamageTaken += dmg;
        if (Health < 0)
        {
            Health = 0;
            OnDeath?.Invoke();
        }
    }

    public virtual void DealDamage(IDamageable damageableEntity, float dmg)
    {
        if (!IsDamageable(this) || dmg < 0) return;
        damageableEntity.TakeDamage(this, dmg);
    }
}
