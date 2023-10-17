using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class AbstractShapeEnemy : LivingEntity
{
    [Space(10f)]
    [Header("AbstractShapeEnemy Fields")]
    [SerializeField] public ShapeEnemyProperties Properties = new();

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
    }
    private void Start()
    {
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
    public override void DealDamage(IDamageable damageableEntity, float dmg)
    {
        base.DealDamage(damageableEntity, dmg);
    }
    private void Move()
    {
        float dst;
        _progress += MoveSpeed * GameUtils.GLOBAL_SPEED_MULTIPLIER *Time.deltaTime;
        dst = _progress / 1;
        transform.position = Properties._pathCreator.path.GetPointAtDistance(_progress / 1f, _endInstruction);
        if((_progress / 1f) >= Properties._pathCreator.path.length)
        {
            //TODO: reached end, destroy and take lives away, if lives are under...
            //print(GameManager.Instance.CurrentLevel.Properties.Lives + " ====> LIVES BEFORE");
            TakeLives(Layer.GetNumLives(CurrentLayer));
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
        if (Health - dmg < 0)
        {
            dmg -= Mathf.Abs(Health - dmg);
        }
        if (damageDealer != null)
            damageDealer.DamageGiven += dmg;
        DamageTaken += dmg;
        if (CurrentLayer == Layer.Layers.White)
        {
            //kill shape
            print("killed");
            OnDeath?.Invoke();
            return;
        }
        print("Old Health " + Health);
        Health -= dmg;
        print("New Health " + Health);
        if (Health <= 0)
        {
            OnLayerSwap?.Invoke();
        }

    }
    public void TakeDamage(IDamageable damageDealer, float dmg, List<DamageTypes> damageTypes)
    {
        print(!CanBeDamaged(damageTypes) + $" can be damaged by {damageTypes} from {damageDealer}?");
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
        CurrentLayer--;
        MoveSpeed = Layer._layerSpeeds[CurrentLayer];
        Health = Layer._layerHealths[CurrentLayer];
        UpdateGraphics();
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
            print("Striped " + CurrentLayer.ToString());
            //enable stripe on it
            Properties._stripe.SetActive(true);
        }
    }
    private void TakeLives(int num)
    {
        AbstractLevel.LevelProperties props = GameManager.Instance.CurrentLevel.Properties;
        props.Lives -= num;
        GameManager.Instance.CurrentLevel.Properties = props;
    }
    [Serializable]
    public class ShapeEnemyProperties
    {
        [SerializeField] internal Layer.Layers _currentLayer;
        [SerializeField] internal float _moveSpeed;
        [SerializeField] internal GameObject _stripe;
        [SerializeField] internal PathCreator _pathCreator;
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
