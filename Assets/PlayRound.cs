using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayRound : MonoBehaviour
{
    [SerializeField] private AbstractLevel _level;
    [SerializeField] private GameObject _playButton, _fastForwardButton;
    private int _numRounds;
    [SerializeField] private int _startRound = 2;
    private Toggle _fastForwardSpeed;
    private Button _play;
    private bool _isOn = false;
    private bool _wasClicked = false;
    private const float _fastTime = 2f;
    private const float _normalTime = 1f;
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
        _fastForwardButton.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(Init_cr());
    }
    private void Update()
    {

    }
    public void ToggleTime()
    {
        //TODO: most likely refactor the fuck out of this
        if (!_fastForwardButton.activeSelf) return;

        if (_fastForwardSpeed.isOn)
        {
            _isOn = true;
            Time.timeScale = _fastTime;
        }
        else
        {
            _isOn = false;
            Time.timeScale = _normalTime;
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
            _wasClicked = false;
            StartRound(AbstractLevel._rounds[i]);
            print("waiting for canstartround");
            yield return new WaitUntil(GameManager.Instance.CanStartNextRound);
            print("setting stuff active again");
            Time.timeScale = _normalTime;
            _playButton.SetActive(true);
            _fastForwardButton.SetActive(false);
            GameManager.Instance.AddMoney(100 + (i + 1)); //add 100 + round num
            print("WAITING FOR BUTTON PRESS");
            yield return new WaitUntil(() => _wasClicked);
            print("NEXT");
        }
        print($"finished all rounds for {_level.Properties._mode} difficulty");
    }
    public void WasClicked()
    {
        _wasClicked = true;
    }
    private void StartRound(Round round)
    {
        _playButton.SetActive(false);
        _fastForwardButton.SetActive(true);
        _fastForwardSpeed.isOn = _isOn;
        if (_isOn) Time.timeScale = _fastTime;
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
        for (int i = 0; i < round.GetWavesInRound().Count; i++)
        {
            StartCoroutine(SpawnWave_cr(round[i]));
        }
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
                MakeShape(prefabPair._prefab, layer, j);
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
            MakeShape(prefab, layer, i);
            yield return new WaitForSeconds(wave.timeBetweenSpawns);
        }
    }
    private static void MakeShape(GameObject prefab, Layer.Layers layer, int namingNumber)
    {
        AbstractShapeEnemy newShape = Instantiate(prefab).GetComponent<AbstractShapeEnemy>();
        newShape.gameObject.name = layer.ToString() + " triangle " + namingNumber;
        GameManager.Instance.CurrentShapesOnScreen.Add(newShape);
    }
}
