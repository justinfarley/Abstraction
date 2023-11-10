using UnityEngine;

public abstract class Debuff : MonoBehaviour
{
    protected float _lifeTime = -1;
    private bool _canFinish = false;
    protected Tower _sourceParent;
    protected AbstractShapeEnemy _shape;
    private void Awake()
    {
        _shape = GetComponent<AbstractShapeEnemy>();
    }
    public virtual void Update()
    {
        if (_lifeTime > 0)
        {
            _canFinish = true;
            _lifeTime -= Time.deltaTime;
            return;
        }
        if (_lifeTime <= 0 && _canFinish) 
        {
            Destroy(this);
        }
    }
    public void SetLifeTime(float time)
    {
        _lifeTime = time;
    }
    public void SetSourceParent(Tower tower)
    {
        _sourceParent = tower;
    }
}
