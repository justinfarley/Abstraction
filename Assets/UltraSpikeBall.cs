using System.Collections.Generic;
using UnityEngine;

public class UltraSpikeBall : SpikeBall
{
    private const float _additionalAttackDamage = 20f;
    private void Start()
    {
        _spikeTower = Properties._sourceParent.GetComponent<SpikeTower>();
        _attackDamage = _spikeTower.Properties._attackDamage + _additionalAttackDamage;
        _pierce = 1;
    }
    /// <summary>
    /// refactored for 4 directions instead of <see cref="SpikeBall.Explode(GameObject)"/> which is 8 directions
    /// </summary>
    /// <param name="prefab">the prefab to spawn (spike balls in this case)</param>
    public override void Explode(GameObject prefab)
    {
        isExploding = true;
        //code to instantiate 4 SPIKE BALLS from the position and make them go the 4 directions:
        //(1,1), (-1,1), (1,-1), (-1,-1)
        Vector2[] dirs =
        {
            new Vector2(1,1), new Vector2(-1,1), new Vector2(-1, -1), new Vector2(1, -1),
        };
        for (int i = 0; i < dirs.Length; i++)
        {
            Projectile projectile = Projectile.InstantiateIndirect(prefab, transform.position, Quaternion.Euler(0, 0, 0), _spikeTower);
            projectile.Properties._dir = dirs[i];
            projectile.Properties._distBeforeDespawn += 1f;
            projectile.isDirect = false;
            projectile.Pierce = 1;
            projectile.transform.rotation = Quaternion.FromToRotation(Vector2.right, dirs[i]);
        }
        Destroy(this.gameObject);
    }
}
