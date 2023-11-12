using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using Unity.VisualScripting;

/// <summary>
/// USES TOWERS NAMES... SEE <see cref="Entity.Name"/> <para></para>
/// MAKE SURE THE FOLDER NAMES IN RESOURCES MATCH THE ENTITY NAMES
/// </summary>
public class UpgradeGUI : AbstractTowerUpgradePaths
{
    public static Action OnCurrentTowerUpdated;
    private static Tower _currentTower;
    private static bool _isActive;
    public static Tower CurrentTower
    {
        get
        {
            return _currentTower;
        }
        set
        {
            _currentTower = value;
            OnCurrentTowerUpdated?.Invoke();
        }
    }

    [Space(10f)]
    [Header("UpgradeGUI Fields")]
    [SerializeField] private GameObject _topPathButton;
    [SerializeField] private GameObject _middlePathButton;
    [SerializeField] private GameObject _bottomPathButton;

    private void Awake()
    {
        OnCurrentTowerUpdated += UpdateGUI;
        GameManager.OnCashChanged += UpdateGUI;

    }
    void Start()
    {
/*        List<Transform> immediateChildren = new List<Transform>();
        for(int i = 0; i < transform.childCount; i++)
        {
            print(i);
            immediateChildren.Add(transform.GetChild(i));
        }
        foreach(var f in immediateChildren)
        {
            f.gameObject.SetActive(false);
        }*/
    }

    void Update()
    {
        
    }
    private void UpdateGUI()
    {
        if (CurrentTower == null || !CurrentTower.Placed)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            _isActive = false;
            return;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            _isActive = true;
            string towerName = _currentTower.Name;
            string topPath = $"SO/Upgrades/{towerName}/TopPath";
            string middlePath = $"SO/Upgrades/{towerName}/MiddlePath";
            string bottomPath = $"SO/Upgrades/{towerName}/BottomPath";
            _topPath = Resources.LoadAll<Upgrade>(topPath);
            _middlePath = Resources.LoadAll<Upgrade>(middlePath);
            _bottomPath = Resources.LoadAll<Upgrade>(bottomPath);
            _topPath = SortPathArray(_topPath);
            _middlePath = SortPathArray(_middlePath);
            _bottomPath = SortPathArray(_bottomPath);
            ButtonInit(_topPathButton, 0, _topPath);
            ButtonInit(_middlePathButton, 1, _middlePath);
            ButtonInit(_bottomPathButton, 2, _bottomPath);
        }
    }
    private Upgrade[] SortPathArray(Upgrade[] upgrades)
    {
        for(int i = 0; i < upgrades.Length; i++)
        {
            for(int j = 0; j < upgrades.Length; j++)
            {
                if (j == i) continue;
                string name1 = upgrades[i].Name;
                string name2 = upgrades[j].Name;
                char char1 = name1[0];
                char char2 = name2[0];

                int num1 = CharUnicodeInfo.GetDecimalDigitValue(char1);
                int num2 = CharUnicodeInfo.GetDecimalDigitValue(char2);


                if (num1 < num2)
                {
                    //swap them
                    (upgrades[j], upgrades[i]) = (upgrades[i], upgrades[j]);
                }
            }
        }
        return upgrades;
    }
    private void ButtonInit(GameObject b, int pathCurrentIndex, Upgrade[] upgradesOnPath)
    {
        if (upgradesOnPath.Length <= _currentTower.GetUpgrades()[pathCurrentIndex])
        {
            //TODO: put like MAXED or something on that path
            //TODO: also add logic for if 2 paths have been purchased already so it can LOCK the other one
            //TODO: also add logic for only 3 upgrades and over on 1 path and 2 and below on 1 other one etc.
            return;
        }
        Button button = b.GetComponent<Button>(); 
        Image upgradeSpriteRenderer = b.transform.GetChild(1).GetComponent<Image>();
        TMP_Text buyText = b.transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text priceText = b.transform.GetChild(2).GetComponent<TMP_Text>();
        TMP_Text nameText = b.transform.GetChild(3).GetComponent<TMP_Text>();
        int currentPathOnTowerIndex = _currentTower.GetUpgrades()[pathCurrentIndex];
        //use index
        print($"{b.name}: {currentPathOnTowerIndex}");
        priceText.text = $"${upgradesOnPath[currentPathOnTowerIndex].Price}";
        nameText.text = $"{upgradesOnPath[currentPathOnTowerIndex].Name}";
        if(GameManager.Instance.CurrentLevel.Properties.Cash >= upgradesOnPath[currentPathOnTowerIndex].Price)
        {
            buyText.text = "BUY";
            button.interactable = true;
        }
        else
        {
            buyText.text = "Cannot buy";
            button.interactable = false;
        }
        upgradeSpriteRenderer.sprite = upgradesOnPath[currentPathOnTowerIndex].upgradeSprite;
        //add upgrades to the button action
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            BuyNextUpgrade(upgradesOnPath, ref _currentTower.GetUpgrades()[pathCurrentIndex]);
        });
    }
    public static void SetCurrentTower(Tower tower)
    {
        _tower = tower;
        CurrentTower = tower;
    }
    public static Tower GetCurrentTower()
    {
        return _tower;
    }
    public static bool IsActive()
    {
        return _isActive;
    }
}
