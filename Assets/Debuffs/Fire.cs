using System.Collections;
using UnityEngine;
public class Fire : Debuff
{
    private float _damage = 1;
    private float _timeBetweenDamage = 0.5f;
    //TODO: add visuals, implementation is done
    public void SetTimeBetweenDamage(float time)
    {
        _timeBetweenDamage = time;
    }
    private void Start()
    {
        _lifeTime = 6f;
        DamageTick();
    }
    private void DamageTick()
    {
        StartCoroutine(DamageTick_cr());
    }
    private IEnumerator DamageTick_cr()
    {
        while(true)
        {
            _shape.TakeDamage(_sourceParent, _damage);
            yield return new WaitForSeconds(_timeBetweenDamage);
        }
    }

}
