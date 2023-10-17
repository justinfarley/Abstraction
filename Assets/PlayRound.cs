using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRound : MonoBehaviour
{
    private AbstractLevel _level;
    private int _numRounds;
    [Serializable]
    public struct LayerPrefabPair
    {
        [SerializeField] internal Layer.Layers _layer;
        [SerializeField] internal GameObject _prefab;
    }
    [SerializeField] private List<LayerPrefabPair> _prefabPairs;

    void Start()
    {
        _level = GameManager.Instance.CurrentLevel;
        //numRounds = level.Properties.finalRound;
        _numRounds = 5;
        StartGame();
    }

    void Update()
    {
        
    }
    private void StartGame()
    {
        StartCoroutine(RoundIterator_cr());
    }
    private IEnumerator RoundIterator_cr()
    {
        for(int i = 0; i < _numRounds; i++)
        {
            StartRound(AbstractLevel._rounds[i]);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.CurrentShapesOnScreen.Count <= 0);
        }
        print("finished all rounds for this difficulty");
    }
    private void StartRound(Round round)
    {
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
        if (numWaves >= round.GetWavesInRound().Count) yield break;
        else
        {
            for (int i = numWaves; i < round.GetWavesInRound().Count; i++)
            {
                StartCoroutine(SpawnWave_cr(round[i]));
            }
        }
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
    private void MakeShape(GameObject prefab, Layer.Layers layer, int namingNumber)
    {
        AbstractShapeEnemy newShape = Instantiate(prefab).GetComponent<AbstractShapeEnemy>();
        newShape.gameObject.name = layer.ToString() + " triangle " + namingNumber;
        GameManager.Instance.CurrentShapesOnScreen.Add(newShape);
    }
}