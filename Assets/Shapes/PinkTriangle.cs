using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkTriangle : AbstractShapeEnemy
{
    public override void Awake()
    {
        base.Awake();
        CurrentLayer = Layer.Layers.Pink;
    }

    public override void Update()
    {
        base.Update();
    }
}
