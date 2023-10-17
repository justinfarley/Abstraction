using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugShapeEnemy : AbstractShapeEnemy
{
    public override void Awake()
    {
        base.Awake();
        CurrentLayer = Layer.Layers.Red; 
    }
    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(null,6);
        }
    }
}
