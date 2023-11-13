using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

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
        GameManager.OnCashChanged += () => OnCurrentTowerUpdated?.Invoke();

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

    private List<int> GetUpgradeablePaths()
    {
        List<int> results = new List<int>();
        int top = _currentTower.GetUpgrades()[0];
        int middle = _currentTower.GetUpgrades()[1];
        int bottom = _currentTower.GetUpgrades()[2];

        HandleTier2And1(top, middle, bottom, results);

        HandleInitialTiers(top, middle, bottom, results);

        HandleMidTiers(top,middle,bottom,results);

        HandleTier3AndAbove(top, middle, bottom, results);

        return results;
    }
    private void HandleMidTiers(int top, int middle, int bottom, List<int> results)
    {
        //this effectively checks 300 -> 502 OR 030 -> 052
        if ((top >= 1 && middle <= 0 && bottom < 2) ||
            (middle >= 1 && top <= 0 && bottom < 2))
        {
            if (!results.Contains(bottom))
                results.Add(bottom);
        }
        //this effectively checks 003 -> 025 AND 300 -> 520
        if ((bottom >= 1 && top <= 0 && middle < 2) ||
            (top >= 1 && bottom <= 0 && middle < 2))
        {
            if (!results.Contains(middle))
                results.Add(middle);
        }
        //this effectively checks 030 -> 250 AND 030 -> 205
        if ((middle >= 1 && bottom <= 0 && top < 2) ||
            (bottom >= 1 && middle <= 0 && top < 2))
        {
            if (!results.Contains(top))
                results.Add(top);
        }
    }
    private void HandleTier3AndAbove(int top, int middle, int bottom, List<int> results)
    {
        if ((top >= 3 && middle == 1) ||
            (top == 1 && middle >= 3))
        {
            if (!results.Contains(top))
                results.Add(top);
            if (!results.Contains(middle))
                results.Add(middle);
        }
        if ((top == 3 && bottom == 1) ||
            (top == 1 && bottom == 3))
        {
            if (!results.Contains(top))
                results.Add(top);
            if (!results.Contains(bottom))
                results.Add(bottom);
        }
        if ((middle >= 3 && bottom == 1) ||
            (middle == 1 && bottom >= 3))
        {
            if (!results.Contains(middle))
                results.Add(middle);
            if (!results.Contains(bottom))
                results.Add(bottom);
        }
        if ((top >= 3 && (middle == 2 || bottom == 2 || middle == 1 || bottom == 1)))
        {
            if (!results.Contains(top))
                results.Add(top);
        }
        if ((middle >= 3 && (bottom == 2 || top == 2 || bottom == 1 || top == 1)))
        {
            if (!results.Contains(middle))
                results.Add(middle);
        }
        if ((bottom >= 3 && (top == 2 || middle == 2 || top == 1 || middle == 1)))
        {
            if (!results.Contains(bottom))
                results.Add(bottom);
        }
    }
    private void HandleInitialTiers(int top, int middle, int bottom, List<int> results)
    {
        //this effectively checks 000 -> 005
        if (top <= 0 && middle <= 0)
        {
            if (!results.Contains(top))
                results.Add(top);
            if (!results.Contains(middle))
                results.Add(middle);
            if (!results.Contains(bottom))
                results.Add(bottom);
        }
        //this effectively checks 000 -> 050
        if (top <= 0 && bottom <= 0)
        {
            if (!results.Contains(top))
                results.Add(top);
            if (!results.Contains(middle))
                results.Add(middle);
            if (!results.Contains(bottom))
                results.Add(bottom);
        }
        //this effectively checks 000 -> 500
        if (middle <= 0 && bottom <= 0)
        {
            if (!results.Contains(top))
                results.Add(top);
            if (!results.Contains(middle))
                results.Add(middle);
            if (!results.Contains(bottom))
                results.Add(bottom);
        }
    }
    private void HandleTier2And1(int top, int middle, int bottom, List<int> results)
    {
        if ((top == 1 && middle == 1) ||
            (top == 2 && middle == 2) ||
            (top == 1 && middle == 2) ||
            (top == 2 && middle == 1))
        {
            if (!results.Contains(top))
                results.Add(top);
            if (!results.Contains(middle))
                results.Add(middle);
        }
        if ((middle == 1 && bottom == 1) ||
        (middle == 2 && bottom == 2) ||
        (middle == 1 && bottom == 2) ||
            (middle == 2 && bottom == 1))
        {
            if (!results.Contains(bottom))
                results.Add(bottom);
            if (!results.Contains(middle))
                results.Add(middle);
        }
        if ((top == 1 && bottom == 1) ||
            (top == 2 && bottom == 2) ||
            (top == 1 && bottom == 2) ||
            (top == 2 && bottom == 1))
        {
            if (!results.Contains(top))
                results.Add(top);
            if (!results.Contains(bottom))
                results.Add(bottom);
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
        List<int> crossPaths = GetUpgradeablePaths();
        print($"Does crossPaths contain {pathCurrentIndex} ? {crossPaths.Contains(_currentTower.GetUpgrades()[pathCurrentIndex])}");
        Button button = b.GetComponent<Button>();
        Image upgradeSpriteRenderer = b.transform.GetChild(1).GetComponent<Image>();
        TMP_Text buyText = b.transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text priceText = b.transform.GetChild(2).GetComponent<TMP_Text>();
        TMP_Text nameText = b.transform.GetChild(3).GetComponent<TMP_Text>();
        if (_currentTower.GetUpgrades()[pathCurrentIndex] >= upgradesOnPath.Length)
        {
            //TODO: put like MAXED or something on that path
            button.interactable = false;
            buyText.text = "Fully Upgraded";
            nameText.text = "";
            priceText.text = "";
            upgradeSpriteRenderer.enabled = false;
            return;
        }
        if (!crossPaths.Contains(_currentTower.GetUpgrades()[pathCurrentIndex]))
        {
            button.interactable = false;
            if(_currentTower.GetUpgrades()[pathCurrentIndex] > 1)
                buyText.text = "Branch Maxed";
            else
                buyText.text = "Upgrade Path Locked";
            nameText.text = "";
            priceText.text = "";
            upgradeSpriteRenderer.enabled = false;
            return;
        }
        //TODO: also add logic for if 2 paths have been purchased already so it can LOCK the other one
        //TODO: also add logic for only 3 upgrades and over on 1 path and 2 and below on 1 other one etc.
        int currentPathOnTowerIndex = _currentTower.GetUpgrades()[pathCurrentIndex];
        //use index
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
