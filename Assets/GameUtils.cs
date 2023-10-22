using System.Collections.Generic;

public static class GameUtils
{
    public static float GLOBAL_SPEED_MULTIPLIER = 1f;

    public static float GLOBAL_SHAPE_HEALTH_MULTIPLIER = 1f;

    public static float GLOBAL_UPGRADE_COST_MULTIPLIER = 1f;

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
