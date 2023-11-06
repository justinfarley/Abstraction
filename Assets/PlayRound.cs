using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Round;

public class PlayRound : MonoBehaviour
{
    public enum State
    {
        WaitingToStart,
        NormalSpeed,
        FastSpeed,
        Paused,
    }
    [SerializeField] private AbstractLevel _level;
    [SerializeField] private GameObject _playButton, _fastForwardButton;
    private int _numRounds;
    [SerializeField] private int _startRound = 2;
    private Toggle _fastForwardSpeed;
    private Button _play;
    private const float _fastTime = 2f;
    private const float _normalTime = 1f;
    private State _state;
    private bool _wasFast = false;
    public static Action OnRoundStarted;
    public static int CurrentRound { get; private set; } = 0;
    public static event Action OnStateChanged;
    [Serializable]
    public struct LayerPrefabPair
    {
        [SerializeField] internal Layer.Layers _layer;
        [SerializeField] internal GameObject _prefab;
    }
    [SerializeField] private List<LayerPrefabPair> _prefabPairs;
    public List<LayerPrefabPair> PrefabPairs { get => _prefabPairs; set => _prefabPairs = value; }
    private void Awake()
    {
        _play = _playButton.GetComponent<Button>();
        _play.onClick.AddListener(StartGame);
        _fastForwardSpeed = _fastForwardButton.GetComponent<Toggle>();
        OnStateChanged += HandleState;
        OnRoundStarted += () => CurrentRound++;
        SwapState(State.WaitingToStart);
    }

    void Start()
    {
        StartCoroutine(Init_cr());
    }
    private void Update()
    {
        //print(_state);
    }
    private void SwapState(State state)
    {
        _state = state;
        OnStateChanged?.Invoke();
    }
    private void HandleState()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                Time.timeScale = _normalTime;
                _playButton.SetActive(true);
                _fastForwardButton.SetActive(false);
                break;
            case State.NormalSpeed:
                Time.timeScale = _normalTime;
                _playButton.SetActive(false);
                _fastForwardButton.SetActive(false);
                _wasFast = false;
                _fastForwardButton.SetActive(true);
                _fastForwardSpeed.isOn = false;
                break;
            case State.FastSpeed:
                _playButton.SetActive(false);
                Time.timeScale = _fastTime;
                _wasFast = true;
                _fastForwardButton.SetActive(true);
                _fastForwardSpeed.isOn = true;
                break;
            case State.Paused:
                break;
            default:
                break;

        }
    }
    public void StartGame()
    {
        _play = _playButton.GetComponent<Button>();
        _play.onClick.RemoveAllListeners();
        _play.onClick.AddListener(WasClicked);
        _playButton.SetActive(false);
        _fastForwardButton.SetActive(true);
        _fastForwardSpeed = _fastForwardButton.GetComponent<Toggle>();
        _fastForwardSpeed.isOn = false; //isOn = false means 1x speed, if its on set Time.timeScale to 2
        SwapState(State.NormalSpeed);
        StartCoroutine(RoundIterator_cr());
    }
    private IEnumerator Init_cr()
    {
        yield return new WaitUntil(() =>
        {
            if(GameManager.Instance.CurrentLevel != null)
            {
                return true;
            }
            return false;
        });
        _level = GameManager.Instance.CurrentLevel;
        _numRounds = _level.Properties._finalRound;
        //StartGame();
    }
    private IEnumerator RoundIterator_cr()
    {
        for (int i = _startRound - 1; i < _numRounds; i++)
        {
            StartRound(AbstractLevel._rounds[i]);
            print("waiting for canstartround");
            yield return new WaitUntil(GameManager.Instance.CanStartNextRound);
            print("setting stuff active again");
            SwapState(State.WaitingToStart);
            GameManager.Instance.AddMoney(100 + (i + 1)); //add 100 + round num
            print("WAITING FOR BUTTON PRESS");
            yield return new WaitUntil(() => _state == State.NormalSpeed || _state == State.FastSpeed);
            print("NEXT");
        }
        print($"finished all rounds for {_level.Properties._mode} difficulty");
    }
    public void WasClicked()
    {
        if(_state == State.NormalSpeed)
        {
            SwapState(State.FastSpeed);
        }
        else if(_state == State.FastSpeed)
        {
            SwapState(State.NormalSpeed);
        }
        else
        {
            if(_wasFast)
            {
                SwapState(State.FastSpeed);
            }
            else
            {
                SwapState(State.NormalSpeed);
            }
        }
    }
    private void StartRound(Round round)
    {
        OnRoundStarted?.Invoke();
        GameManager.Instance.CurrentRound = round;
        switch (round.SpawnType)
        {
            case Round.WaveSpawnType.Seperate:
                StartCoroutine(StartRound_cr(round, round.GetWavesInRound().Count));
                break;
            case Round.WaveSpawnType.Together:
                SpawnAllWaves(round);
                break;
            case Round.WaveSpawnType.TogetherAtWave:
                StartCoroutine(StartRound_cr(round, round.WaveToSpawnTogether));
                break;
        }

    }
    private void SpawnAllWaves(Round round)
    {
        StartCoroutine(SpawnAllWaves_cr(round));
    }
    private IEnumerator SpawnAllWaves_cr(Round round)
    {
        for (int i = 0; i < round.GetWavesInRound().Count - 1; i++)
        {
            StartCoroutine(SpawnWave_cr(round[i]));
        }
        //TODO: make donespawning trigger after all the shapes are spawned
        yield return StartCoroutine(SpawnWave_cr(round[round.GetWavesInRound().Count - 1]));
        round.DoneSpawning = true;
    }
    private IEnumerator StartRound_cr(Round round, int numWaves)
    {
        for (int i = 0; i < numWaves; i++)
        {
            for (int j = 0; j < round[i].shapes; j++) 
            {
                Layer.Layers layer = round[i].layer;
                LayerPrefabPair prefabPair = new();
                foreach (var pair in _prefabPairs)
                {
                    if (pair._layer == layer)
                    {
                        prefabPair = pair;
                        break;
                    }
                }
                //instantiate shape of specified color
                try
                {
                    MakeShape(prefabPair._prefab, layer, j, round[i].variant);
                }
                catch (Exception)
                {
                    Debug.LogError($"You forgot to add the LayerPrefab pair for the {layer} layer.");
                }
                yield return new WaitForSeconds(round[i].timeBetweenSpawns);
            }
        }
        if (numWaves >= round.GetWavesInRound().Count)
        {
            print("done spawning everything");
            round.DoneSpawning = true;
            yield break;
        }
        else
        {
            for (int i = numWaves; i < round.GetWavesInRound().Count; i++)
            {
                StartCoroutine(SpawnWave_cr(round[i]));
            }
        }
        //done spawning everything, round over.
        print("done spawning everything");
        round.DoneSpawning = true;
    }
    private IEnumerator SpawnWave_cr(Round.Wave wave)
    {
        for (int i = 0; i < wave.shapes; i++)//for each shape in wave
        {
            Layer.Layers layer = wave.layer;
            GameObject prefab = null;
            foreach (var pair in _prefabPairs)
            {
                if (pair._layer == layer)
                {
                    prefab = pair._prefab;
                    break;
                }
            }
            //instantiate shape of specified color
            try
            {
                MakeShape(prefab, layer, i, wave.variant);
            }
            catch(Exception)
            {
                Debug.LogError($"You forgot to add the LayerPrefab pair for the {wave.layer} layer.");
            }
            yield return new WaitForSeconds(wave.timeBetweenSpawns);
        }
    }
    private static void MakeShape(GameObject prefab, Layer.Layers layer, int namingNumber, AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant variant)
    {
        AbstractShapeEnemy newShape = Instantiate(prefab).GetComponent<AbstractShapeEnemy>();
        newShape.gameObject.name = layer.ToString() + " triangle " + namingNumber;
        newShape.Properties._shapeVariant = variant;
        newShape.ChangedVariant(variant);
        GameManager.Instance.CurrentShapesOnScreen.Add(newShape);
    }
}
