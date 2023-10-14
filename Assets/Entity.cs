using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public enum Type
    {
        None,
        Player,
        Enemy,
        Item,
        Static,
        LevelTrigger,
        Projectile,
    }
    public enum State
    {
        None,
        Alive,
        Dead,
        Invulnerable,
    }
    public State CurrentState { get => entityProperties.state; set => entityProperties.state = value; }

    public Type EntityType { get => entityProperties.type; set => entityProperties.type = value; }

    public Sprite Sprite { get => entityProperties.sprite; set => entityProperties.sprite = value; }

    public Collider2D EntityCollider { get => entityProperties.entityCollider; set => entityProperties.entityCollider = value; }

    public Rigidbody2D EntityRigidbody { get => entityProperties.entityRigidbody; set => entityProperties.entityRigidbody = value; }
    public SpriteRenderer SpriteRenderer { get => entityProperties.spriteRenderer; set => entityProperties.spriteRenderer = value; }

    public Animator Animator { get => entityProperties.animator; set => entityProperties.animator = value; }
    public string Name { get => name; set => name = value; }
     
    [Space(10f)]
    [Header("Entity Fields")]
    public EntityProperties entityProperties = new();

    [Serializable]
    public class EntityProperties
    {
        [SerializeField] internal string name;
        [SerializeField] internal Rigidbody2D entityRigidbody;
        [SerializeField] internal Collider2D entityCollider;
        [SerializeField] internal Sprite sprite;
        [SerializeField] internal SpriteRenderer spriteRenderer;
        [SerializeField] internal Type type;
        [SerializeField] internal State state;
        [SerializeField] internal Animator animator;
        public EntityProperties() 
        {
            name = "";
            entityRigidbody = null;
            entityCollider = null;
            sprite = null;
            type = Type.None;
            state = State.None;
            animator = null;
        }
        public EntityProperties(string name, Rigidbody2D entityRigidbody, Collider2D entityCollider, Sprite sprite, Type type, State state, Animator animator)
        {
            this.name = name;
            this.entityRigidbody = entityRigidbody;
            this.entityCollider = entityCollider;
            this.sprite = sprite;
            this.type = type;
            this.state = state;
            this.animator = animator;
        }
    }

    public static bool IsDamageable(Entity entity)
    {
        if (entity is IDamageable) return true;
        return false;
    }

}
