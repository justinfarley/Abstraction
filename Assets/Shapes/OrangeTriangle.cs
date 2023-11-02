using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeTriangle : AbstractShapeEnemy
{
    public override void Awake()
    {
        base.Awake();
        CurrentLayer = Layer.Layers.Orange;
    }

    public override void Update()
    {
        base.Update();
    }
}
