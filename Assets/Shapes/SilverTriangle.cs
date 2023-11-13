using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverTriangle : AbstractShapeEnemy
{
    public override void Awake()
    {
        base.Awake();
        CurrentLayer = Layer.Layers.Silver;
        //TODO: remove this when there is something that can damage it
        Properties._typesToBeDamagedBy.Add(DamageTypes.All);
    }

    public override void Update()
    {
        base.Update();
    }
}
