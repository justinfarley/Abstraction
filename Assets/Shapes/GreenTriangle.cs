using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTriangle : AbstractShapeEnemy
{
    public override void Awake()
    {
        base.Awake();
        CurrentLayer = Layer.Layers.Green;
    }

    public override void Update()
    {
        base.Update();
    }
}
