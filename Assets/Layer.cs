using System.Collections.Generic;

public static class Layer
{
    internal static Dictionary<Layers, float> layerHealths = new Dictionary<Layers, float>()
    {
        { Layers.White, 1f },
        { Layers.Yellow, 2f },
        { Layers.Blue, 2f },
        { Layers.Red, 2f },
        { Layers.Green, 2f },
        { Layers.Orange, 3f },
        { Layers.Pink, 3f },
        { Layers.Brown, 3f },
        { Layers.Purple, 4f },
        { Layers.Silver, 1f },
        { Layers.Black, 10f },
    };
    public enum Layers
    {
        White = 1,
        Yellow = 2,
        Blue = 3,
        Red = 4,
        Green = 5,
        Orange = 6,
        Pink = 7,
        Brown = 8,
        Purple = 9,
        Silver = 10,
        Black = 11,
        WhiteStriped = 12,
        YellowStriped = 13,
        BlueStriped = 14,
        RedStriped = 15,
        GreenStriped = 16,
        OrangeStriped = 17,
        PinkStriped = 18,
        BrownStriped = 19,
        PurpleStriped = 20,
        SilverStriped = 21,
        BlackStriped = 22,
    }

}
    

