using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownTriangle : AbstractShapeEnemy
{
    public override void Awake()
    {
        base.Awake();
        CurrentLayer = Layer.Layers.Brown;
    }

    public override void Update()
    {
        base.Update();
    }
}
