using UnityEngine;

public abstract class AbstractTowerUpgradePaths : MonoBehaviour
{
    //START WITH 3 UPGRADES PER PATH
    [Space(10f)]
    [Header("Upgrade Path Fields")]
    [SerializeField] protected Tower _tower;
    [SerializeField] protected AbstractUpgrade[] _topPath = new AbstractUpgrade[3];
    [SerializeField] protected AbstractUpgrade[] _middlePath = new AbstractUpgrade[3];
    [SerializeField] protected AbstractUpgrade[] _bottomPath = new AbstractUpgrade[3];
    protected int _topPathIndex = 0;
    protected int _middlePathIndex= 0;
    protected int _bottomPathIndex = 0;
    public virtual void BuyNextUpgrade(AbstractUpgrade[] path, ref int nextIndex)
    {
        if (GameManager.Instance.Money < path[nextIndex].Properties._price) return; //guard clause
        GameManager.Instance.Money -= path[nextIndex].Properties._price;
        _tower.Properties._attackDamage += path[nextIndex].Properties._damageIncrease;
        _tower.Properties._attackRadius += path[nextIndex].Properties._rangeIncrease;
        _tower.Properties._attackSpeed += path[nextIndex].Properties._attackSpeedIncrease;
        foreach(var dmgType in path[nextIndex].Properties._damageTypesToAdd)
        {
            _tower.Properties._damageTypes.Add(dmgType);
        }
        print(_topPathIndex);
        nextIndex++;
        print(_topPathIndex);
    }
}
