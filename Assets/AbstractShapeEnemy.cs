using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public abstract class AbstractShapeEnemy : LivingEntity
{
    [Space(10f)]
    [Header("AbstractShapeEnemy Fields")]
    [SerializeField] public ShapeEnemyProperties Properties = new();
    private PlayRound _round;
    private readonly EndOfPathInstruction _endInstruction = EndOfPathInstruction.Stop;
    public Layer.Layers CurrentLayer { get => Properties._currentLayer; set => Properties._currentLayer = value; }
    public float MoveSpeed { get => Properties._moveSpeed; set => Properties._moveSpeed = value; }

    public PathCreator Path { get => Properties._pathCreator; set => Properties._pathCreator = value; }

    public float Progress { get => (_progress / 1); set => _progress = value; }

    private float _progress = 0f;
    public Action OnLayerSwap;
    public override void Awake()
    {
        base.Awake();
        EntityType = Type.ShapeEnemy;
        OnDeath += () =>
        {
            GameManager.Instance.CurrentShapesOnScreen.Remove(this);
            Destroy(gameObject);
        };  
        OnLayerSwap += SwitchLayer;
        Properties._camoSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Shapes/triangle_camo.png");
        Properties._normalSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Shapes/triangle_normal.png");
        Properties._regenSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites//Shapes/triangle_regen.png  ");
        
    }
    private void Start()
    {
        _round = FindObjectOfType<PlayRound>();
        Path = FindObjectOfType<PathCreator>();
        MoveSpeed = Layer._layerSpeeds[CurrentLayer];
        Health = Layer._layerHealths[CurrentLayer];
        CurrentState = State.Alive;

        UpdateGraphics();
    }
    public virtual void Update()
    {
        Move();
    }
    public void ChangedVariant(ShapeEnemyProperties.ShapeVariant variant)
    {
        SpriteRenderer.sprite = (variant) switch
        {
            ShapeEnemyProperties.ShapeVariant.Normal => Properties._normalSprite,
            ShapeEnemyProperties.ShapeVariant.Regen => Properties._regenSprite,
            ShapeEnemyProperties.ShapeVariant.Camo => Properties._camoSprite,
            _ => null,
        };
    }
    public override void DealDamage(IDamageable damageableEntity, float dmg)
    {
        base.DealDamage(damageableEntity, dmg);
    }
    private void Move()
    {
        float dst;
        _progress += MoveSpeed * GameUtils.GLOBAL_SPEED_MULTIPLIER * Time.deltaTime;
        dst = _progress;
        transform.position = Properties._pathCreator.path.GetPointAtDistance(_progress, _endInstruction);
        if((_progress) >= Properties._pathCreator.path.length)
        {
            //TODO: reached end, destroy and take lives away, if lives are under...
            //print(GameManager.Instance.CurrentLevel.Properties.Lives + " ====> LIVES BEFORE");
            GameManager.Instance.TakeLives(Layer.GetNumLives(CurrentLayer));
            //print(GameManager.Instance.CurrentLevel.Properties.Lives + " ====> LIVES AFTER");
            OnDeath?.Invoke();
        }
    }

    //TODO: When damaging the shapes: formula can be something like base damage * poppingPower, make certain layers more durable. More durable = pierce does more damage, normal dmg does less damage etc.
    //TODO: implement pierce -> in Projectile code, pierce of 3 = projectile can hit 3 things before dissapearing
    public override void TakeDamage(IDamageable damageDealer, float dmg)
    {
        if (dmg < 0) return;
        if (CurrentState == State.Invulnerable) return;
        if (CurrentLayer == Layer.Layers.White)
        {
            //kill shape
            print("killed");
            if (damageDealer != null)
                damageDealer.DamageGiven += 1;
            DamageTaken += 1;
            GameManager.Instance.AddMoney(1);
            OnDeath?.Invoke();
            return;
        }
        float leftoverDamage = 0;
        if (Health - dmg < 0)
        {
            leftoverDamage = Mathf.Abs(Health - dmg);
        }
        if (damageDealer != null)
            damageDealer.DamageGiven += dmg - leftoverDamage;
        DamageTaken += dmg - leftoverDamage;
        Health -= dmg - leftoverDamage;
        GameManager.Instance.AddMoney((int)(dmg - leftoverDamage));
        if (Health <= 0)
        {
            OnLayerSwap?.Invoke();
            if(leftoverDamage > 0)
            {
                TakeDamage(damageDealer, leftoverDamage);
            }
        }

    }
    public void TakeDamage(IDamageable damageDealer, float dmg, List<DamageTypes> damageTypes)
    {
        if (!CanBeDamaged(damageTypes)) return;
        else TakeDamage(damageDealer, dmg);
    }
    private bool CanBeDamaged(List<DamageTypes> damageTypes)
    {
        if (Properties._typesToBeDamagedBy.Contains(DamageTypes.All)) return true;
        foreach(var type in Properties._typesToBeDamagedBy)
        {
            foreach(var damageType in damageTypes)
            {
                if (type == damageType) return true;
            }
        }
        return false;
    }
    public virtual void SwitchLayer()
    {
        CheckSpawnExtraShapes(CurrentLayer);
        CurrentLayer--;
        MoveSpeed = Layer._layerSpeeds[CurrentLayer];
        Health = Layer._layerHealths[CurrentLayer];
        UpdateGraphics();
    }

    private void CheckSpawnExtraShapes(Layer.Layers currentLayer)
    {
        int numSplits = Layer._layerSplitCounts[currentLayer];
        numSplits--;
        if(numSplits <= 0 || currentLayer == Layer.Layers.White) return;
        for (int i = 0; i < numSplits; i++)
        {
            AbstractShapeEnemy newShape = Instantiate(_round.PrefabPairs[(int)currentLayer - 2]._prefab, Vector3.zero, Quaternion.identity).GetComponent<AbstractShapeEnemy>();
            float buffer = 0.5f;
            newShape.Progress = Progress - (buffer * (i+1));
            newShape.name = "Split";
        }   
        
    }

    private void UpdateGraphics()
    {
        //update graphics
        if ((int)CurrentLayer <= 11)
        { //all non-striped shapes
            SpriteRenderer.color = Properties.colors[(int)(CurrentLayer - 1)];
            Properties._stripe.SetActive(false);
        }
        else
        {
            print(Properties.colors[(int)(CurrentLayer - 12)]);
            SpriteRenderer.color = Properties.colors[(int)(CurrentLayer - 12)];
            //enable stripe on it
            Properties._stripe.SetActive(true);
        }
    }
    public bool IsCamo()
    {
        return Properties._shapeVariant == ShapeEnemyProperties.ShapeVariant.Camo;
    }

    [Serializable]
    public class ShapeEnemyProperties
    {
        internal Layer.Layers _currentLayer;
        internal float _moveSpeed;
        internal PathCreator _pathCreator;
        internal Sprite _camoSprite, _regenSprite, _normalSprite;
        [SerializeField] internal GameObject _stripe;
        [SerializeField] internal List<DamageTypes> _typesToBeDamagedBy;
        [SerializeField] internal ShapeVariant _shapeVariant;
        public enum ShapeVariant
        {
            Normal,
            Regen,
            Camo,
        }

        internal Color32[] colors =
        {
            new Color32(255,255,255,255), //white
            new Color32(235, 201, 52, 255), //yellow
            new Color32(0, 0, 255, 255), //blue
            new Color32(255, 0, 0, 255), //red
            new Color32(0,171,0,255), //green
            new Color32(255,125,0,255), //orange
            new Color32(255, 59, 209,255), //pink
            new Color32(92, 47, 21,255), //brown
            new Color32(164, 0, 209,255), //purple
            new Color32(105, 105, 105,255), //silver
            new Color32(0,0,0,255), //Black
        };
    }
}
