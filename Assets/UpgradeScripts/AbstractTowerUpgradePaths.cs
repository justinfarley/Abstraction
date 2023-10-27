using UnityEngine;

public abstract class AbstractTowerUpgradePaths : MonoBehaviour
{
    //START WITH 3 UPGRADES PER PATH
    [Space(10f)]
    [Header("Upgrade Path Fields")]
    [SerializeField] protected static Tower _tower;
    [SerializeField] protected Upgrade[] _topPath;
    [SerializeField] protected Upgrade[] _middlePath;
    [SerializeField] protected Upgrade[] _bottomPath;
    protected int _topPathIndex = 0;
    protected int _middlePathIndex= 0;
    protected int _bottomPathIndex = 0;
    public virtual void BuyNextUpgrade(Upgrade[] path, ref int nextIndex)
    {
        if (GameManager.Instance.CurrentLevel.Properties.Cash < path[nextIndex].Price) return; //guard clause
        GameManager.Instance.AddMoney(-path[nextIndex].Price);
        _tower.Properties._attackDamage += path[nextIndex].DamageIncrease;

        print(_tower.Properties._attackRadius);
        _tower.Properties._attackRadius += path[nextIndex].RangeIncrease;
        _tower.Properties._radius.transform.localScale = new Vector3(_tower.Properties._attackRadius, _tower.Properties._attackRadius, _tower.Properties._attackRadius);
        print(_tower.Properties._attackRadius);

        _tower.Properties._attackSpeed -= path[nextIndex].AttackDelayDecrease;
        _tower.Properties._projectileSpeed += path[nextIndex].ProjectileSpeedIncrease;
        _tower.Properties._projectileTravelDistance += path[nextIndex].ProjectileTravelDistanceIncrease;
        if (!_tower.Properties._canHitCamo) //only change if you couldnt before the upgrade
        {
            _tower.Properties._canHitCamo = path[nextIndex].canHitCamo;
        }
        foreach(var dmgType in path[nextIndex].DamageTypesToAdd)
        {
            if(!_tower.Properties._damageTypes.Contains(dmgType))
                _tower.Properties._damageTypes.Add(dmgType);
        }
        nextIndex++;
        UpgradeGUI.OnCurrentTowerUpdated?.Invoke();
    }
}
