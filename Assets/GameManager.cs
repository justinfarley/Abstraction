using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    public List<AbstractShapeEnemy> CurrentShapesOnScreen = new List<AbstractShapeEnemy>();
    public AbstractLevel CurrentLevel { get => _currentLevel; set => _currentLevel = value; }

    private AbstractLevel _currentLevel = null;
    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }
    private void OnLevelWasLoaded(int level)
    {
        
    }
    public void AddMoney(int amount)
    {
        AbstractLevel.LevelProperties props = GameManager.Instance.CurrentLevel.Properties;
        props.Cash += amount;
        Instance.CurrentLevel.Properties = props;
    }
    public void TakeLives(int num)
    {
        AbstractLevel.LevelProperties props = GameManager.Instance.CurrentLevel.Properties;
        props.Lives -= num;
        Instance.CurrentLevel.Properties = props;
    }
}
