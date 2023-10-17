using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteTriangle : AbstractShapeEnemy
{
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        CurrentLayer = Layer.Layers.White;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
