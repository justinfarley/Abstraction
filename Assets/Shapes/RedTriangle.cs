using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTriangle : AbstractShapeEnemy
{
    public override void Awake()
    {
        base.Awake();
        CurrentLayer = Layer.Layers.Red;
    }

    public override void Update()
    {
        base.Update();
    }
}
