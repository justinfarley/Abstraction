using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractLevel : MonoBehaviour
{
    private const AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant CAMO = AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant.Camo;
    private const AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant REGEN = AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant.Regen;

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
            _startingCash = 500;
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
    internal static List<Round> _rounds = new List<Round>()
    {
        //Round 1
        new Round(
      new List<Round.Wave>(){
                Round.MakeWave(10, 1f, Layer.Layers.White),
            },Round.WaveSpawnType.Seperate
        ),
        //Round 2: Introduce Yellows
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(3, 2f, Layer.Layers.Yellow),
                 Round.MakeWave(10, 0.75f, Layer.Layers.White),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 3
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(5, 0.6f, Layer.Layers.Yellow),
                 Round.MakeWave(20, 0.4f, Layer.Layers.White),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 4: introduce Blues
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(5, 1f, Layer.Layers.Blue),
                 Round.MakeWave(10, 0.5f, Layer.Layers.Yellow),
            },Round.WaveSpawnType.Together
            ),
        //Round 5: 
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(10, 1f, Layer.Layers.Blue),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 6:
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(10, 2f, Layer.Layers.Blue),
                 Round.MakeWave(50, 0.25f, Layer.Layers.White),
            },Round.WaveSpawnType.Together
            ),
        //Round 7:
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(5, 5f, Layer.Layers.Red),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 8:
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(5, 5f, Layer.Layers.Red),
                 Round.MakeWave(10, 2.5f, Layer.Layers.Red),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 9:
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(5, 1f, Layer.Layers.Green),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 10:
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(5, 2f, Layer.Layers.Green),
                 Round.MakeWave(10, 1f, Layer.Layers.Red),
                 Round.MakeWave(25, 0.5f, Layer.Layers.White),
            },Round.WaveSpawnType.Together
            ),
        //Round 11:
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(50, 0.25f, Layer.Layers.Red),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 12:
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(50, 0.05f, Layer.Layers.White),
                 Round.MakeWave(10, 1f, Layer.Layers.Green),
                 Round.MakeWave(20, 0.25f, Layer.Layers.Blue),
            },Round.WaveSpawnType.Together
            ),
        //Round 13:
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(200, 0.05f, Layer.Layers.White),
                 Round.MakeWave(20, 0.25f, Layer.Layers.Orange),
            },Round.WaveSpawnType.Together
            ),
        //Round 14:
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(5, 0.5f, Layer.Layers.Pink),
                 Round.MakeWave(10, 0.25f, Layer.Layers.Pink),
                 Round.MakeWave(20, 0.1f, Layer.Layers.Pink),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 15:
        new Round(
        new List<Round.Wave>(){
                 Round.MakeWave(1, 0f, Layer.Layers.Red, CAMO),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 16:
        new Round(
        new List<Round.Wave>(){
                Round.MakeWave(25, 0.1f, Layer.Layers.Green),
                Round.MakeWave(25, 0.1f, Layer.Layers.Orange),
                Round.MakeWave(25, 0.1f, Layer.Layers.Pink),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 17:
        new Round(
        new List<Round.Wave>(){
                Round.MakeWave(50, 0.25f, Layer.Layers.Green),
                Round.MakeWave(10, 1f, Layer.Layers.Brown),
            },Round.WaveSpawnType.Together
            ),
        //Round 18:
        new Round(
        new List<Round.Wave>(){
                Round.MakeWave(50, 0.25f, Layer.Layers.Green, CAMO),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 19:
        new Round(
        new List<Round.Wave>(){
                Round.MakeWave(50, 0.25f, Layer.Layers.Green, REGEN),
                Round.MakeWave(25, 0.5f, Layer.Layers.Blue, REGEN),
                Round.MakeWave(10, 1f, Layer.Layers.White, REGEN),
                Round.MakeWave(20, 1f, Layer.Layers.Red, CAMO),
            },2
            ),
        //Round 20:
        new Round(
        new List<Round.Wave>(){
                Round.MakeWave(1, 0.5f, Layer.Layers.White),
                Round.MakeWave(2, 0.5f, Layer.Layers.Yellow),
                Round.MakeWave(4, 0.5f, Layer.Layers.Blue),
                Round.MakeWave(8, 0.25f, Layer.Layers.Red),
                Round.MakeWave(16, 0.25f, Layer.Layers.Green),
                Round.MakeWave(32, 0.2f, Layer.Layers.Orange),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 21:
        new Round(
        new List<Round.Wave>(){
                Round.MakeWave(100, 0.375f, Layer.Layers.Orange, REGEN),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 22: TEST ROUND
        new Round(
        new List<Round.Wave>(){
                Round.MakeWave(1, 1f, Layer.Layers.Blue, REGEN),
            },Round.WaveSpawnType.Seperate
            ),
    };
}
