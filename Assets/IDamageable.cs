using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public float DamageGiven { get; set; }
    public float DamageTaken { get; set; }
    public void TakeDamage(IDamageable damageDealer, float dmg);

    public void DealDamage(IDamageable damageableEntity, float dmg);
}
