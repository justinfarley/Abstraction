using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tower;

public class SpikeBall : Projectile
{
    private float _attackDamage;
    private SpikeTower _spikeTower;
    private void Start()
    {
        _spikeTower = Properties._sourceParent.GetComponent<SpikeTower>();
        _attackDamage = _spikeTower.Properties._attackDamage + 5f;
    }
    private void FixedUpdate()
    {
        print(_pierce);
        transform.Rotate(new Vector3(0, 0, Random.Range(-2,3)));
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<AbstractShapeEnemy>())
        {
            AbstractShapeEnemy enemy = collision.gameObject.GetComponent<AbstractShapeEnemy>();
            print($"Exploding at {enemy.name}");
            DealDamage(enemy, _spikeTower.Properties._damageTypes);
        }
    }
    public override void DealDamage(AbstractShapeEnemy damageableEntity, List<DamageTypes> damageTypes)
    {
        damageableEntity.TakeDamage(Properties._sourceParent, _attackDamage, damageTypes);
        _pierce--;
        if (_pierce <= 0) Destroy(gameObject);
    }
    private void OnDestroy()
    {
        print("EXPLODING");
        //code to instantiate 8 spikes from the position and make them go the 8 directions:
        //(1,1), (1,0), (0,1), (-1, 0), (0,-1), (-1,1), (1,-1), (-1,-1)
        Vector2[] dirs =
        {
            Vector2.right, Vector2.left, Vector2.up, Vector2.down, 
            new Vector2(1,1), new Vector2(-1,1), new Vector2(-1, -1), new Vector2(1, -1),
        };
        int[] zRots =
        {
            0, 180, 90, 270,
            45, 135, 225, 315
        };
        for(int i = 0; i < 8; i++)
        {
            Projectile projectile = Projectile.Instantiate(_spikeTower.Properties._projectilePrefab, transform.position, Quaternion.identity, _spikeTower);
            projectile.name = $"{_spikeTower.Properties._damageTypes} Projectile";
            projectile.Properties._dir = dirs[i];
            projectile.Properties._distBeforeDespawn += 5f;
            projectile.transform.rotation = Quaternion.Euler(0, 0, zRots[i]);
        }
    }
}
