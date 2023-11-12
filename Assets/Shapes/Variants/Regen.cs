using System.Collections;
using UnityEngine;

public class Regen : MonoBehaviour
{
    private float _timeBeforeRegrowStart = 1f; //this number + the GameUtils.GLOBAL_REGROW_RATE_IN_SECONDS is the total time before starting the regrow every GameUtils.GLOBAL_REGROW_RATE_IN_SECONDS seconds
    private Layer.Layers _baseLayer;
    private AbstractShapeEnemy _enemy;
    private Coroutine _regen;
    private void Awake()
    {
        _enemy = GetComponent<AbstractShapeEnemy>();
        _baseLayer = _enemy.CurrentLayer;
    }
    private void Start()
    {
        _enemy.OnLayerSwap += () =>
        {
            StartRegen();
        };
    }
    public void StartRegen()
    {   if(_regen == null)
            _regen = StartCoroutine(Regen_cr());
        else
        {
            StopAllCoroutines();
            _regen = null;
            _regen = StartCoroutine(Regen_cr());
        }
    }
    private IEnumerator Regen_cr()
    {
        print("Restarted Coroutine");
        yield return new WaitUntil(() => _enemy.GetTimeNotDamaged() >= _timeBeforeRegrowStart);
        while (true)
        {
            yield return new WaitForSeconds(GameUtils.GLOBAL_REGROW_RATE_IN_SECONDS);
            Regrow();
        }
    }
    private void Regrow()
    {
        _enemy.RegenLayer();
        //print($"New Layer : {_enemy.CurrentLayer} ---- Base regrow layer: {_baseLayer}");
        if(_enemy.CurrentLayer == _baseLayer)
        {
            StopAllCoroutines();
            _regen = null;
        }
    }
    public void SetBaseLayer(Layer.Layers layer)
    {
        _baseLayer = layer;
    }
    public Layer.Layers GetBaseLayer()
    {
        return _baseLayer;
    }
}
