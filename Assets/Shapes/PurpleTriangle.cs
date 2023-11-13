using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleTriangle : AbstractShapeEnemy
{
    //TODO: only damaged by certain stuff maybe make base class for shapes only damaged by certain types
    public override void Awake()
    {
        base.Awake();
        CurrentLayer = Layer.Layers.Purple;
        //TODO: remove this when there is something that can damage it
        Properties._typesToBeDamagedBy.Add(DamageTypes.All);
    }

    public override void Update()
    {
        base.Update();
    }
}
