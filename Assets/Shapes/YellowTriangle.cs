using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowTriangle : AbstractShapeEnemy
{
    public override void Awake()
    {
        base.Awake();
        CurrentLayer = Layer.Layers.Yellow;
    }

    public override void Update()
    {
        base.Update();
    }
}
