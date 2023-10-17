using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class AbstractUpgrade
{
    [Space(10f)]
    [Header("Upgrade Fields")]
    [SerializeField] public UpgradeProperties Properties;

    [Serializable]
    public struct UpgradeProperties
    {
        [SerializeField] internal string _name;
        [SerializeField] internal string _description;
        [SerializeField] internal int _price;
        [SerializeField] internal int _pierceIncrease;
        [SerializeField] internal float _damageIncrease;
        [SerializeField] internal float _attackSpeedIncrease;
        [SerializeField] internal float _rangeIncrease;
        [SerializeField] internal List<DamageTypes> _damageTypesToAdd;

        //UpgradeState
    }
}
