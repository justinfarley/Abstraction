using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTriangle : AbstractShapeEnemy
{
    public override void Awake()
    {
        base.Awake();
        CurrentLayer = Layer.Layers.Blue;
    }

    public override void Update()
    {
        base.Update();
    }
}
