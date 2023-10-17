using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTowerUpgrades : AbstractTowerUpgradePaths
{
    private void Awake()
    {
        _tower = GetComponent<SpikeTower>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BuyNextUpgrade(_topPath, ref _topPathIndex);
        }
    }
}
