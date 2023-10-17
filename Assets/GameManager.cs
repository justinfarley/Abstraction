using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    public List<AbstractShapeEnemy> CurrentShapesOnScreen = new List<AbstractShapeEnemy>();
    public AbstractLevel CurrentLevel { get => _currentLevel; set => _currentLevel = value; }

    private AbstractLevel _currentLevel = null;
    private int _money = 0;
    public int Money { get => _money; set => _money = value; }
    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }
    private void OnLevelWasLoaded(int level)
    {
        
    }
}
