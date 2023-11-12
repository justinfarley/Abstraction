using System.Collections.Generic;
using System.Linq;
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
        if (nextIndex >= path.Length) return;
        if (GameManager.Instance.CurrentLevel.Properties.Cash < path[nextIndex].Price) return; //guard clause
        GameManager.Instance.AddMoney(-path[nextIndex].Price);
        print(_tower);
        _tower.Properties._attackDamage += path[nextIndex].DamageIncrease;

        _tower.AddSpecialUpgrade(path[nextIndex].SpecialUpgrade);
        _tower.Properties._attackRadius += path[nextIndex].RangeIncrease;
        _tower.Properties._radius.transform.localScale = new Vector3(_tower.Properties._attackRadius, _tower.Properties._attackRadius, _tower.Properties._attackRadius);

        _tower.TryDecreaseAttackSpeed(path[nextIndex].AttackDelayDecrease);
        //_tower.Properties._attackSpeed -= path[nextIndex].AttackDelayDecrease;

        _tower.Properties._projectilePierce += path[nextIndex].PierceIncrease;
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
        foreach (var v in path[nextIndex].DebuffsToAdd)
        {
            _tower.Properties._debuffsToGive.Add(v);
        }
        nextIndex++;
        int[] upgradeInts = _tower.GetUpgrades();
        string upgradeString = "" + upgradeInts[0] + upgradeInts[1] + upgradeInts[2];
        SwapSprite(upgradeString,_tower);
        UpgradeGUI.OnCurrentTowerUpdated?.Invoke();
    }
    private void SwapSprite(string upgrades, Tower tower)
    {
        TowerUpgradeSprites upgradeSprites = tower.GetTowerUpgradeSprites();
        List<Sprite> spriteArr = TowerUpgradeSprites.GetAllSprites(upgradeSprites);
        for (int i = 0; i < spriteArr.Count; i++)
        {
            if (spriteArr[i].name.Substring(0, 3).Equals(upgrades))
            {
                _tower.SpriteRenderer.sprite = spriteArr[i];
            }
        }
    }
}
