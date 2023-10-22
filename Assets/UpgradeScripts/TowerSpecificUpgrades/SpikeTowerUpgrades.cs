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
        //TODO: implement buttons to buy each path, update the upgrade shown based on the index of each one. Add logic for not being able to go down all 3 paths, only over 3 in one path and lock the other one once 3 is reached on one etc.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BuyNextUpgrade(_topPath, ref _topPathIndex);
        }
    }
}
