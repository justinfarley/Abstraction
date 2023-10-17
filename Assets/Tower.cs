using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : LivingEntity
{
    [Space(10f)]
    [Header("Tower Fields")]
    public TowerProperties Properties;
    [Header("Gizmos Options")]
    [SerializeField] private Color _gizmosColor;
    private List<AbstractShapeEnemy> _shapesInRange;
    private AbstractShapeEnemy _nextAttackableShape;
    private Collider2D[] _hits;
    public Color GizmosColor { get => _gizmosColor; set => _gizmosColor = value; }
    public AbstractShapeEnemy NextAttackableShape { get => _nextAttackableShape; set => _nextAttackableShape = value; }
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
        //Just for now the logic is going in update.
        //TODO: Eventually make the radius a collider, and each time a shape enters the radius, update the _shapesInRange list
        _hits = Physics2D.OverlapCircleAll(transform.position, Properties._attackRadius);
        _shapesInRange = FindShapesInRange(_hits);
        _nextAttackableShape = GetNextAttackableShape(_shapesInRange);
        if(_nextAttackableShape != null )
        {
            transform.right = _nextAttackableShape.transform.position - transform.position;
        }
    }
    protected void Init_Tower(float attackSpeed, float attackRadius, float attackDamage, DamageTypes damageType, int pierce, TowerProperties.AttackType attackType, GameObject projectilePrefab, float projectileSpeed)
    {
        Properties._attackSpeed = attackSpeed;
        Properties._attackRadius = attackRadius;
        Properties._attackDamage = attackDamage;
        Properties._damageTypes.Add(damageType);
        Properties._projectilePierce = pierce;
        Properties._attackType = attackType;
        Properties._projectileSpeed = projectileSpeed;
        Properties._projectilePrefab = projectilePrefab;
    }
    private AbstractShapeEnemy GetNextAttackableShape(List<AbstractShapeEnemy> shapesInRange)
    {
        if (shapesInRange == null) return null;
        if (shapesInRange.Count <= 0) return null;
        return (Properties._attackType) switch
        {
            TowerProperties.AttackType.First => GetFirstShape(shapesInRange),
            TowerProperties.AttackType.Last => GetLastShape(shapesInRange),
            TowerProperties.AttackType.Strong => GetStrongShape(shapesInRange),
            TowerProperties.AttackType.Weak => GetWeakShape(shapesInRange),
            TowerProperties.AttackType.Close => GetCloseShape(shapesInRange),
            TowerProperties.AttackType.Far => GetFarShape(shapesInRange),
            _ => null,
        };
    }
    private AbstractShapeEnemy GetFirstShape(List<AbstractShapeEnemy> shapesInRange)
    {
        int index = 0;
        for(int i = index; i < shapesInRange.Count; i++)
        {
            if (Properties._variantsAbleToAttack.Contains(shapesInRange[i].Properties._shapeVariant)) return shapesInRange[i];
        }
        return null;
    }
    private AbstractShapeEnemy GetLastShape(List<AbstractShapeEnemy> shapesInRange)
    {
        int index = shapesInRange.Count - 1;
        for (int i = index; i > shapesInRange.Count; i--)
        {
            if (Properties._variantsAbleToAttack.Contains(shapesInRange[i].Properties._shapeVariant)) return shapesInRange[i];
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
                if (Properties._variantsAbleToAttack.Contains(shape.Properties._shapeVariant) && Layer.GetNumLives(shape.CurrentLayer) > Layer.GetNumLives(strongest.CurrentLayer))
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
                if (Properties._variantsAbleToAttack.Contains(shape.Properties._shapeVariant) && Layer.GetNumLives(shape.CurrentLayer) < Layer.GetNumLives(weakest.CurrentLayer))
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
                    Properties._variantsAbleToAttack.Contains(shape.Properties._shapeVariant))
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
                    Properties._variantsAbleToAttack.Contains(shape.Properties._shapeVariant))
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
    private List<AbstractShapeEnemy> FindShapesInRange(Collider2D[] hits)
    {
        //TODO: refactor so that the list is ordered based on how far along the PATH they are.
        //Either sort after this foreach loop or check during the loop.
        List<AbstractShapeEnemy> list = new List<AbstractShapeEnemy>();
        foreach (var col in hits)
        {
            if(col.GetComponent<AbstractShapeEnemy>() != null)
            {
                list.Add(col.GetComponent<AbstractShapeEnemy>());
            }
        }
        if(list.Count <= 0) return null;
        for (int i = 0; i < list.Count; i++)
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
                return true;
            });
            //attack
            print($"Next damageable: {_nextAttackableShape.name}");
            Attack();
            //DealDamage(_nextAttackableShape, Properties._attackDamage, Properties._damageType);
            yield return new WaitForSeconds(Properties._attackSpeed);
        }
    }
    public virtual void OnDrawGizmos()
    {
        if (Properties._isSelected)
        {
            Gizmos.color = _gizmosColor;
            Gizmos.DrawWireSphere(transform.position, Properties._attackRadius);
        }
    }
    [Serializable]
    public struct TowerProperties
    {
        [SerializeField] internal float _attackSpeed;
        [SerializeField] internal GameObject _projectilePrefab;
        [SerializeField] internal float _attackRadius;
        [SerializeField] internal float _attackDamage;
        [SerializeField] internal bool _isSelected;
        [SerializeField] internal AttackType _attackType;
        [SerializeField] internal List<DamageTypes> _damageTypes;
        [SerializeField] internal int _projectilePierce;
        [SerializeField] internal float _projectileSpeed;
        [SerializeField] internal List<AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant> _variantsAbleToAttack;
        public enum AttackType
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
