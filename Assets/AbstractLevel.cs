using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AbstractLevel : MonoBehaviour
{

    [SerializeField] protected LevelProperties _levelProperties;
    public LevelProperties Properties { get => _levelProperties; set => _levelProperties = value; }


    public virtual void Awake()
    {

    }
    public virtual void Start()
    {
        GameManager.Instance.CurrentLevel = this;
        GameUtils.GLOBAL_UPGRADE_COST_MULTIPLIER = GameUtils.PriceMultipliers[GameManager.Instance.CurrentLevel._levelProperties._mode];
        LevelProperties props = GameManager.Instance.CurrentLevel.Properties;
        props.Cash = Properties.StartingCash;
        GameManager.Instance.CurrentLevel.Properties = props;
    }

    [Serializable]
    public class LevelProperties
    {
        [SerializeField] internal string _name;
        [SerializeField] internal string _description;
        [SerializeField] internal int _finalRound;
        public LevelProperties(Mode mode)
        {
            this._mode = mode;
            _name = "";
            _description = "";
            _finalRound = 0;
            _cash = 0;
            _startingCash = 600;
            switch (this._mode)
            {
                case Mode.Baby:
                    _lives = 200;
                    _finalRound = 40;
                    break;
                case Mode.Easy:
                    _lives = 150;
                    _finalRound = 40;
                    break;
                case Mode.Medium:
                    _finalRound = 60;
                    _lives = 100;
                    break;
                case Mode.Hard:
                    _finalRound = 100;
                    _lives = 100;
                    break;
                case Mode.FasterShapes:
                    _finalRound = 100;
                    _lives = 100;
                    break;
                case Mode.DoubleHealthDiamonds:
                    _finalRound = 100;
                    _lives = 100;
                    break;
                case Mode.Abstract:
                    _lives = 1;
                    _finalRound = 120;
                    break;
                default:
                    _lives = -1;
                    _finalRound = -1;
                    Debug.LogError("ERROR: NO MODE SELECTED");
                    break;
            }
            _description = Description();
        }
        public enum Mode
        {
            None,
            Baby,   //easiest
            Easy,
            Medium,
            Hard,
            FasterShapes,
            DoubleHealthDiamonds,
            Abstract,//hardest
        }
        public string Description()
        {
            return _mode switch
            {
                Mode.Baby => "If you lose on this difficulty... seek help.",
                Mode.Easy => "Easy mode. Towers are cheap.",
                Mode.Medium => "Start with less lives. Towers cost slightly more.",
                Mode.Hard => "Start with just 100 lives. Towers are more expensive. Only skilled players survive.",
                Mode.FasterShapes => "Same as hard mode, but all shapes move 100% faster. Good luck.",
                Mode.DoubleHealthDiamonds => "Same as hard mode, but all Diamonds have double the health.",
                Mode.Abstract => "Only for Abstraction experts. 1 life. 1 chance.",
                Mode.None => "No mode selected.",
                _ => "An Error Occurred. No mode was chosen.",
            };
        }
        [SerializeField] internal Mode _mode;
        public Action OnCashAmountChanged;
        public Action OnLivesAmountChanged;
        private int _lives;
        public int Lives { get => _lives; set
            {
                _lives = value;
                OnLivesAmountChanged?.Invoke();
            }
        }
        private int _cash;
        public int Cash 
        {
            get => _cash; 
            set 
            { 
                _cash = value;
                OnCashAmountChanged?.Invoke();
            } }
        private int _startingCash;
        public int StartingCash { get => _startingCash; set => _startingCash = value; }
    }
}
