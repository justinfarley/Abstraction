using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugShapeEnemy : AbstractShapeEnemy
{
    public override void Awake()
    {
        base.Awake();
        CurrentLayer = Layer.Layers.BlackStriped;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(null,0);
        }
    }
}
