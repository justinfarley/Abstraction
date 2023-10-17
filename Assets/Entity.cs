using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public enum Type
    {
        None,
        Tower,
        ShapeEnemy,
        Item,
        Static,
        Projectile,

    }
    public enum State
    {
        None,
        Alive,
        Dead,
        Invulnerable,
    }
    public State CurrentState { get => entityProperties._state; set => entityProperties._state = value; }

    public Type EntityType { get => entityProperties._type; set => entityProperties._type = value; }

    public Sprite Sprite { get => entityProperties._sprite; set => entityProperties._sprite = value; }

    public Collider2D EntityCollider { get => entityProperties._entityCollider; set => entityProperties._entityCollider = value; }

    public Rigidbody2D EntityRigidbody { get => entityProperties._entityRigidbody; set => entityProperties._entityRigidbody = value; }
    public SpriteRenderer SpriteRenderer { get => entityProperties._spriteRenderer; set => entityProperties._spriteRenderer = value; }

    public Animator Animator { get => entityProperties._animator; set => entityProperties._animator = value; }
    public string Name { get => name; set => name = value; }
     
    [Space(10f)]
    [Header("Entity Fields")]
    public EntityProperties entityProperties;

    [Serializable]
    public struct EntityProperties
    {
        [SerializeField] internal string _name;
        [SerializeField] internal Rigidbody2D _entityRigidbody;
        [SerializeField] internal Collider2D _entityCollider;
        [SerializeField] internal Sprite _sprite;
        [SerializeField] internal SpriteRenderer _spriteRenderer;
        [SerializeField] internal Type _type;
        [SerializeField] internal State _state;
        [SerializeField] internal Animator _animator;
    }

    public static bool IsDamageable(Entity entity)
    {
        if (entity is IDamageable) return true;
        return false;
    }

}
