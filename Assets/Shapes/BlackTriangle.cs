using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackTriangle : AbstractShapeEnemy
{
    public override void Awake()
    {
        base.Awake();
        CurrentLayer = Layer.Layers.Black;
        //TODO: remove this when there is something that can damage it
        Properties._typesToBeDamagedBy.Add(DamageTypes.All);
    }

    public override void Update()
    {
        base.Update();
    }
}
