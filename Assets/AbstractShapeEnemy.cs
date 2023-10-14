using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class AbstractShapeEnemy : LivingEntity
{
    [Space(10f)]
    [Header("AbstractShapeEnemy Fields")]
    [SerializeField] public ShapeEnemyProperties properties = new();
    public Layer.Layers CurrentLayer { get => properties.currentLayer; set => properties.currentLayer = value; }

    public override void Awake()
    {
        base.Awake();
        OnDeath += () => Destroy(this.gameObject);
    }
    private void Start()
    {
        UpdateGraphics();
    }
    public override void DealDamage(IDamageable damageableEntity, float dmg)
    {
        base.DealDamage(damageableEntity, dmg);

    }
    //TODO: When damaging the shapes: formula can be something like base damage * pierce, make certain layers more durable. More durable = pierce does more damage, normal dmg does less damage etc.
    public override void TakeDamage(IDamageable damageDealer, float dmg)
    {
        if (!IsDamageable(this) || dmg < 0) return;
        if (CurrentState == State.Invulnerable) return;
        if (CurrentLayer == Layer.Layers.White)
        {
            //kill shape
            OnDeath?.Invoke();
            return;
        }
        Health -= dmg;
        if(Health <= 0)
        {
            Health = 0;
            SwitchLayer();
        }
        UpdateGraphics();
    }
    private void SwitchLayer()
    {
        CurrentLayer--;
        //Health = Layer.layerHealths[CurrentLayer];
    }
    private void UpdateGraphics()
    {
        //update graphics
        if ((int)CurrentLayer <= 11)
        { //all non-striped shapes
            SpriteRenderer.color = properties.colors[(int)(CurrentLayer - 1)];
            properties.stripe.SetActive(false);
        }
        else
        {
            print(properties.colors[(int)(CurrentLayer - 12)]);
            SpriteRenderer.color = properties.colors[(int)(CurrentLayer - 12)];
            print("Striped " + CurrentLayer.ToString());
            //enable stripe on it
            properties.stripe.SetActive(true);
        }
    }
    [Serializable]
    public class ShapeEnemyProperties
    {
        [SerializeField] internal Layer.Layers currentLayer;
        [SerializeField] internal GameObject stripe;
        internal Color32[] colors =
        {
            new Color32(255,255,255,255), //white
            new Color32(235, 201, 52, 255), //yellow
            new Color32(0, 0, 255, 255), //blue
            new Color32(255, 0, 0, 255), //red
            new Color32(0,255,0,255), //green
            new Color32(255,125,0,255), //orange
            new Color32(255, 59, 209,255), //pink
            new Color32(92, 47, 21,255), //brown
            new Color32(164, 0, 209,255), //purple
            new Color32(105, 105, 105,255), //silver
            new Color32(0,0,0,255), //Black
        };
    }
}
