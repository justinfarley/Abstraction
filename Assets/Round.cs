using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Round
{
    [SerializeField] private List<Wave> _wavesInRound = new();
    [SerializeField] private int _currentWave;
    private int _waveToSpawnTogether;
    private bool _doneSpawning = false;
    public bool DoneSpawning { get => _doneSpawning; set => _doneSpawning = value; }
    public int WaveToSpawnTogether { get => _waveToSpawnTogether; private set => _waveToSpawnTogether = value; }
    public enum WaveSpawnType
    {
        Seperate,
        Together,
        TogetherAtWave,
    }
    private WaveSpawnType _spawnType;
    public WaveSpawnType SpawnType { get => _spawnType; set => _spawnType = value; }
    public Wave this[int index]
    {
        get
        {
            return _wavesInRound[index];
        }
        set
        {
            _wavesInRound[index] = value;
        }
    }
    public Round(List<Wave> waves)
    {
        _wavesInRound = waves;
        _currentWave = 0;
        _waveToSpawnTogether = -1;
    }
    public Round(List<Wave> waves, WaveSpawnType waveSpawning)
    {
        _wavesInRound = waves;
        _currentWave = 0;
        _spawnType = waveSpawning;
        _waveToSpawnTogether = -1;
    }
    public Round(List<Wave> waves, int waveToStartSpawningTogether)
    {
        _wavesInRound = waves;
        _currentWave = 0;
        _spawnType = WaveSpawnType.TogetherAtWave;
        _waveToSpawnTogether = waveToStartSpawningTogether;
    }
    public void AddWave(Wave wave)
    {
        _wavesInRound.Add(wave);
    }
    public Wave GetCurrentWave()
    {
        return this[_currentWave];
    }
    public static Wave GetCurrentWave(Round round)
    {
        return round[round._currentWave];
    }
    public List<Wave> GetWavesInRound()
    {
        return _wavesInRound;
    }
    public static Wave MakeWave(int numShapes, float timeBetweenSpawns, Layer.Layers layer)
    {
        return new Wave(numShapes, timeBetweenSpawns, layer);
    }
    public static Wave MakeWave(int numShapes, float timeBetweenSpawns, Layer.Layers layer, AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant variant)
    {
        return new Wave(numShapes, timeBetweenSpawns, layer, variant);
    }
    /// <summary>
    /// Wave of shapes
    /// E.G. 5 Yellow shapes with 0.25 time between each one getting instantiated would look like this:<para></para>
    /// Wave redShapes;<para></para>
    /// redShapes.shapes = 5;<para></para>
    /// redShapes.timeBetweenSpawns = 0.25f;<para></para>
    /// redShapes.layer = Layer.Layers.Red;
    /// </summary>
    [Serializable]
    public struct Wave
    {
        [SerializeField] internal int shapes;
        [SerializeField] internal float timeBetweenSpawns;
        [SerializeField] internal Layer.Layers layer;
        [SerializeField] internal AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant variant;
        public Wave(int shapes, float timeBetweenSpawns, Layer.Layers layer, AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant variant)
        {
            this.shapes = shapes;
            this.timeBetweenSpawns = timeBetweenSpawns;
            this.layer = layer;
            this.variant = variant;
        }
        public Wave(int shapes, float timeBetweenSpawns, Layer.Layers layer)
        {
            this.shapes = shapes;
            this.timeBetweenSpawns = timeBetweenSpawns;
            this.layer = layer;
            variant = AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant.Normal;
        }
    }
}
