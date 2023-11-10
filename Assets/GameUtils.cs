using System.Collections.Generic;

public static class GameUtils
{
    //These are all attributes of the shapes ... e.g the shapes speed, health, etc
    public static float GLOBAL_SPEED_MULTIPLIER = 1f;

    public static float GLOBAL_SHAPE_HEALTH_MULTIPLIER = 1f;

    public static float GLOBAL_WORN_DEBUFF_DMG_TAKEN_MULTIPLIER = 1.5f; //50% more damage so 1.5x

    public static float GLOBAL_SLOWED_DEBUFF_SPEED_REDUCTION_MULTIPLIER = 0.5f;


    //These are all attributes of the towers ... e.g. the upgrade cost of towers, the atk speed cap, etc.
    public static float GLOBAL_UPGRADE_COST_MULTIPLIER = 1f;

    public static float GLOBAL_ATTACK_SPEED_CAP = 0.05f;

    //These are more general stuff
    public static Dictionary<AbstractLevel.LevelProperties.Mode, float> PriceMultipliers = new Dictionary<AbstractLevel.LevelProperties.Mode, float>
    {
        { AbstractLevel.LevelProperties.Mode.Baby, 0.5f },
        { AbstractLevel.LevelProperties.Mode.Easy, 0.75f },
        { AbstractLevel.LevelProperties.Mode.Medium, 1f },
        { AbstractLevel.LevelProperties.Mode.Hard, 1.5f },
        { AbstractLevel.LevelProperties.Mode.FasterShapes, 1.5f },
        { AbstractLevel.LevelProperties.Mode.DoubleHealthDiamonds, 1.5f },
        { AbstractLevel.LevelProperties.Mode.Abstract, 1.75f },
    };
}
