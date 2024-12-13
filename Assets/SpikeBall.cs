using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpikeBall : Projectile
{
    protected float _attackDamage;
    protected SpikeTower _spikeTower;
    [SerializeField] protected GameObject spawnedPrefab;
    private const float _additionalAttackDamage = 5f;
    protected bool isExploding = false;
    private void Start()
    {
        _spikeTower = Properties._sourceParent.GetComponent<SpikeTower>();
        _attackDamage = _spikeTower.Properties._attackDamage + _additionalAttackDamage;
    }
    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, 7));
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<AbstractShapeEnemy>())
        {
            AbstractShapeEnemy enemy = collision.gameObject.GetComponent<AbstractShapeEnemy>();
            DealDamage(enemy, _spikeTower.Properties._damageTypes);
        }
    }
    public override void DealDamage(AbstractShapeEnemy damageableEntity, List<DamageTypes> damageTypes)
    {
        damageableEntity.TakeDamage(Properties._sourceParent, _attackDamage, damageTypes);
        _pierce--;
        if (_pierce <= 0 && !isExploding) Explode(spawnedPrefab);
    }
    private void OnDestroy()
    {
        //Explode(spawnedPrefab);
    }
    public virtual void Explode(GameObject prefab)
    {
        isExploding = true;
        print("EXPLODING");
        //code to instantiate 8 spikes from the position and make them go the 8 directions:
        //(1,1), (1,0), (0,1), (-1, 0), (0,-1), (-1,1), (1,-1), (-1,-1)
        Vector2[] dirs =
        {
            Vector2.right, Vector2.left, Vector2.up, Vector2.down,
            new Vector2(1,1), new Vector2(-1,1), new Vector2(-1, -1), new Vector2(1, -1),
        };
        for (int i = 0; i < 8; i++)
        {
            Projectile projectile = Projectile.InstantiateIndirect(prefab, transform.position, Quaternion.Euler(0,0,0), _spikeTower);
            projectile.Properties._dir = dirs[i];
            projectile.Properties._distBeforeDespawn += 5f;
            projectile.isDirect = false;
            projectile.transform.rotation = Quaternion.FromToRotation(Vector2.right, dirs[i]);
            projectile.Pierce = 200;
        }
        Destroy(this.gameObject);
    }
}
