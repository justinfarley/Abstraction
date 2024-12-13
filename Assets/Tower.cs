using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Tower : LivingEntity
{
    [Space(10f)]
    [Header("Tower Fields")]
    public TowerProperties Properties;
    [SerializeField] private TowerUpgradeSprites _upgradeSprites;
    [Header("Gizmos Options")]
    [SerializeField] private Color _gizmosColor;
    private List<AbstractShapeEnemy> _shapesInRange;
    private AbstractShapeEnemy _nextAttackableShape;
    private Collider2D[] _hits;
    protected int[] _upgrades = { 0, 0, 0 };
    internal List<SpecialUpgrades> _specialUpgrades = new List<SpecialUpgrades>();
    public Color GizmosColor { get => _gizmosColor; set => _gizmosColor = value; }
    public AbstractShapeEnemy NextAttackableShape { get => _nextAttackableShape; set => _nextAttackableShape = value; }
    public bool Placed = false;
    public bool CanBePlaced { get; set; }
    private int currentCollisions = 0;
    public PlaceState PlacingState;
    [SerializeField] protected int _basePrice;

    public enum PlaceState
    {
        NotPlacing,
        Placing,
        Placed,
    }
    public override void Awake()
    {
        base.Awake();
        EntityType = Type.Tower;
        //TEST STATS
        /*        Properties._attackType = TowerProperties.AttackType.First;
                Properties._attackDamage = 1;
                Properties._attackSpeed = 1;
                Properties._damageType = DamageTypes.Spikes;*/
        StartCoroutine(Attack_cr());
    }
    public virtual void Update()
    {
        if (Properties._isSelected && !Placed)
        {
            if (!CanBePlaced)
            {
                Properties._radius.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                Properties._radius.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        if (!Placed)
        {
            if (currentCollisions == 0) CanBePlaced = true;
            else CanBePlaced = false;
            switch (PlacingState)
            {
                case PlaceState.Placing:
                    Vector2 mousePos = Input.mousePosition;
                    Vector2 mouseToWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
                    transform.position = mouseToWorldPos;

                    break;
                case PlaceState.Placed:
                    Placed = true;
                    break;
            }
            return;
        }
        _hits = null;
        _shapesInRange = null;
        _nextAttackableShape = null;
        //Just for now the logic is going in update.
        //TODO: Eventually make the radius a collider, and each time a shape enters the radius, update the _shapesInRange list
        _hits = Physics2D.OverlapCircleAll(transform.position, Properties._attackRadius);
        _shapesInRange = FindShapesInRange(_hits);
/*        string s = $"Shapes in range: ";
        foreach (var v  in _shapesInRange)
        {
            s += v.ToString() + " ";
        }
        print(s);*/
        _nextAttackableShape = GetNextAttackableShape(_shapesInRange);
        if(_nextAttackableShape != null )
        {
            transform.right = _nextAttackableShape.transform.position - transform.position;
        }

        HandleSelection();

    }
    private void HandleSelection()
    {
        if (!Placed) return;
        if (Input.GetMouseButtonDown(0))
        {
            //TODO: refactor to be a raycast at the mouse position and if it hits this towers collider then call TowerSelected.
            if(Input.mousePosition.x > (Screen.width / 5f) * 4 && UpgradeGUI.IsActive())
                return;
            Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(point, transform.position) < 0.5f)
            {
                TowerSelected();
            }
            else
            {
                TowerDeselected();
            }
        }
    }
    public void TowerSelected()
    {
        if (Properties._isSelected) return;
        Properties._isSelected = true;
        Properties._radius.SetActive(true);
        if(Placed)
            UpgradeGUI.SetCurrentTower(this);
    }
    public void TowerDeselected()
    {
        if (!Properties._isSelected) return;
        Properties._isSelected = false;
        Properties._radius.SetActive(false);
        if(UpgradeGUI.GetCurrentTower() == this && Placed)
            UpgradeGUI.SetCurrentTower(null);

    }
    protected void Init_Tower(float attackSpeed, float attackRadius, float attackDamage, List<DamageTypes> damageTypes, int pierce, TowerProperties.Targeting attackType, GameObject projectilePrefab, float projectileSpeed, float travelDistance)
    {
        Properties._attackSpeed = attackSpeed;
        Properties._attackRadius = attackRadius;
        Properties._attackDamage = attackDamage;
        foreach(var i in damageTypes)
            Properties._damageTypes.Add(i);
        Properties._projectilePierce = pierce;
        Properties._attackType = attackType;
        Properties._projectileSpeed = projectileSpeed;
        Properties._projectilePrefab = projectilePrefab;
        Properties._projectileTravelDistance = travelDistance;
        NextAttackableShape = null;
    }
    private AbstractShapeEnemy GetNextAttackableShape(List<AbstractShapeEnemy> shapesInRange)
    {
        if (shapesInRange == null) return null;
        if (shapesInRange.Count <= 0) return null;
        if(_hits == null) return null;
        if(_hits.Length <= 0) return null;
        return (Properties._attackType) switch
        {
            TowerProperties.Targeting.First => GetFirstShape(shapesInRange),
            TowerProperties.Targeting.Last => GetLastShape(shapesInRange),
            TowerProperties.Targeting.Strong => GetStrongShape(shapesInRange),
            TowerProperties.Targeting.Weak => GetWeakShape(shapesInRange),
            TowerProperties.Targeting.Close => GetCloseShape(shapesInRange),
            TowerProperties.Targeting.Far => GetFarShape(shapesInRange),
            _ => null,
        };
    }
    private AbstractShapeEnemy GetFirstShape(List<AbstractShapeEnemy> shapesInRange)
    {
        int index = 0;
        for(int i = index; i < shapesInRange.Count; i++)
        {
            if (shapesInRange[i].IsCamo())
            {
                if(Properties._canHitCamo)
                    return shapesInRange[i];
            }
            else
            {
                return shapesInRange[i];
            }
        }
        return null;
    }
    private AbstractShapeEnemy GetLastShape(List<AbstractShapeEnemy> shapesInRange)
    {
        int index = shapesInRange.Count - 1;
        for (int i = index; i > shapesInRange.Count; i--)
        {
            if (shapesInRange[i].IsCamo())
            {
                if (Properties._canHitCamo)
                    return shapesInRange[i];
            }
            else
            {
                return shapesInRange[i];
            }
        }
        return null;
    }
    private AbstractShapeEnemy GetStrongShape(List<AbstractShapeEnemy> shapesInRange)
    {
        AbstractShapeEnemy strongest = null;
        foreach(var shape in shapesInRange)
        {
            if (strongest == null) strongest = shape;
            else
            {
                if (((shape.IsCamo() && Properties._canHitCamo) || !shape.IsCamo()) && 
                    Layer.GetNumLives(shape.CurrentLayer) > Layer.GetNumLives(strongest.CurrentLayer))
                {
                    strongest = shape;
                }
            }
        }
        return strongest;
    }
    private AbstractShapeEnemy GetWeakShape(List<AbstractShapeEnemy> shapesInRange)
    {
        AbstractShapeEnemy weakest = null;
        foreach (var shape in shapesInRange)
        {
            if (weakest == null) weakest = shape;
            else
            {
                if (((shape.IsCamo() && Properties._canHitCamo) || !shape.IsCamo()) && 
                    Layer.GetNumLives(shape.CurrentLayer) > Layer.GetNumLives(weakest.CurrentLayer))
                {
                    weakest = shape;
                }
            }
        }
        return weakest;
    }
    private AbstractShapeEnemy GetCloseShape(List<AbstractShapeEnemy> shapesInRange)
    {
        AbstractShapeEnemy closest = null;
        foreach (var shape in shapesInRange)
        {
            if (closest == null) closest = shape;
            else 
            {
                if(Vector2.Distance(shape.transform.position, transform.position) < 
                    Vector2.Distance(closest.transform.position, transform.position) &&
                    ((shape.IsCamo() && Properties._canHitCamo) || !shape.IsCamo()))
                {
                    closest = shape;
                }
            }
        }
        return closest;
    }
    private AbstractShapeEnemy GetFarShape(List<AbstractShapeEnemy> shapesInRange)
    {
        AbstractShapeEnemy farthest = null;
        foreach (var shape in shapesInRange)
        {
            if (farthest == null) farthest = shape;
            else
            {
                if (Vector2.Distance(shape.transform.position, transform.position) >
                    Vector2.Distance(farthest.transform.position, transform.position) &&
                    ((shape.IsCamo() && Properties._canHitCamo) || !shape.IsCamo()))
                {
                    farthest = shape;
                }
            }
        }
        return farthest;
    }
    public override void DealDamage(IDamageable damageableEntity, float dmg)
    {
        base.DealDamage(damageableEntity, dmg);
    }
    public void DealDamage(AbstractShapeEnemy damageableEntity, float dmg, List<DamageTypes> damageTypes)
    {
        damageableEntity.TakeDamage(this, dmg, damageTypes);
    }
    public void AddSpecialUpgrade(SpecialUpgrades upgrade)
    {
        if(upgrade != SpecialUpgrades.None)
            _specialUpgrades.Add(upgrade);
    }
    private List<AbstractShapeEnemy> FindShapesInRange(Collider2D[] hits)
    {
        //TODO: refactor so that the list is ordered based on how far along the PATH they are.
        //Either sort after this foreach loop or check during the loop.
        if(hits == null) return null;
        List<AbstractShapeEnemy> list = new List<AbstractShapeEnemy>();
        foreach (var col in hits)
        {
            if(col.GetComponent<AbstractShapeEnemy>() != null)
            {
                list.Add(col.GetComponent<AbstractShapeEnemy>());
            }
        }
        if(list.Count <= 0) return null;
        for (int i = 0; i < list.Count; i++) //SORT LIST BY FIRST TO LAST PROGRESS
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (i == j) continue;
                if (list[j].Progress < list[i].Progress)
                {
                    //swap list positions
                    (list[j], list[i]) = (list[i], list[j]);
                }
            }
        }
        return list;
    }
    protected bool ShapeInRange(Collider2D[] hits)
    {
        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponent<AbstractShapeEnemy>() != null)
            {
                return true;
            }
        }
        return false;
    }
    public abstract void Attack();
    private IEnumerator Attack_cr()
    {
        while (true)
        {
            yield return new WaitUntil(() => 
            {
                if (_shapesInRange == null) return false;
                if(_shapesInRange.Count <= 0) return false;
                if(_hits == null) return false;
                if(_hits.Length <= 0) return false;
                if (!Placed) return false;
                return true;
            });
            //attack
            if (_nextAttackableShape)
            {
                Attack();
            }
            //DealDamage(_nextAttackableShape, Properties._attackDamage, Properties._damageType);
            yield return new WaitForSeconds(Properties._attackSpeed);
        }
    }
    public int[] GetUpgrades()
    {
        return _upgrades;
    }
    public TowerUpgradeSprites GetTowerUpgradeSprites()
    {
        return _upgradeSprites;
    }
    public void TryDecreaseAttackSpeed(float amount)
    {
        float newAm = Properties._attackSpeed - amount;
        if (newAm < GameUtils.GLOBAL_ATTACK_DELAY_CAP)
            Properties._attackSpeed = GameUtils.GLOBAL_ATTACK_DELAY_CAP;
        else
            Properties._attackSpeed -= amount;
    }
    public virtual void OnDrawGizmos()
    {
        if (Properties._isSelected)
        {
            Gizmos.color = _gizmosColor;
            Gizmos.DrawWireSphere(transform.position, Properties._attackRadius);
        }
    }
    public void AddTowerDebuffsToShape(AbstractShapeEnemy enemy)
    {
        foreach(Debuffs d in Properties._debuffsToGive)
        {
            enemy.OnDebuffAdded?.Invoke(d, this);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentCollisions++;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        currentCollisions--;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9) //is the collided object a piece of the track?
        {
            CanBePlaced = false;
        }
    }
    private void OnMouseUp()
    {
        if (Placed) return;
        if (CanBePlaced)
        {
            Placed = true;
            GameUtils.IsPlacing = false;
            TowerDeselected();
            PlacingState = PlaceState.Placed;
        }
    }
    public int GetBasePrice()
    {
        return _basePrice;
    }
    protected void UpdateRadiusGraphics()
    {
        Properties._radius.transform.localScale = new Vector3(Properties._attackRadius, Properties._attackRadius, Properties._attackRadius);
    }
    [Serializable]
    public struct TowerProperties
    {
        [SerializeField] internal float _attackSpeed;
        [SerializeField] internal GameObject _projectilePrefab;
        [SerializeField] internal GameObject _radius;
        [SerializeField] internal float _attackRadius;
        [SerializeField] internal float _attackDamage;
        [SerializeField] internal bool _isSelected;
        [SerializeField] internal Targeting _attackType;
        [SerializeField] internal List<DamageTypes> _damageTypes;
        [SerializeField] internal List<Debuffs> _debuffsToGive;
        [SerializeField] internal int _projectilePierce;
        [SerializeField] internal float _projectileSpeed;
        [SerializeField] internal float _projectileTravelDistance;
        [SerializeField] internal bool _canHitCamo;
        public enum Targeting
        {
            First,
            Last,
            Strong,
            Weak,
            Close,
            Far
        }
    }
}
