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
    public float Health { get; set; }
    public float DamageGiven { get => _damageGiven; set => _damageGiven = value; }

    private float _damageGiven;
    public float DamageTaken { get => _damageTaken; set => _damageTaken = value; }

    private float _damageTaken;

    public Action OnDeath;


    public virtual void Awake()
    {
        Init_LivingEntity();
    }
    private void Init_LivingEntity()
    {
        OnDeath = null;
        CurrentState = State.Alive;
        OnDeath += () => CurrentState = State.Dead;
    }
    public virtual void TakeDamage(IDamageable damageDealer, float dmg)
    {
        if (dmg < 0) return;
        if (CurrentState == State.Invulnerable) return;
        print("Old Health " + Health);
        if (Health - dmg < 0)
        {
            dmg -= Mathf.Abs(Health - dmg);
        }
        Health -= dmg;
        print("New Health " + Health);
        if (damageDealer != null)
            damageDealer.DamageGiven += dmg;
        DamageTaken += dmg;
        if (Health <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public virtual void DealDamage(IDamageable damageableEntity, float dmg)
    {
        if (dmg < 0) return;
        damageableEntity.TakeDamage(this, dmg);
    }
}
