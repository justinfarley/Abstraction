using System.Collections.Generic;

public static class Layer
{
    internal static Dictionary<Layers, int> _layerSplitCounts = new Dictionary<Layers, int>()
    {
        { Layers.White, 1 },
        { Layers.Yellow, 1 },
        { Layers.Blue, 1 },
        { Layers.Red, 1 },
        { Layers.Green, 1 },
        { Layers.Orange, 1 },
        { Layers.Pink, 1 },
        { Layers.Brown, 2 },
        { Layers.Purple, 1 },
        { Layers.Silver, 2 },
        { Layers.Black, 2 },
        { Layers.WhiteStriped, 1 },
        { Layers.YellowStriped, 1 },
        { Layers.BlueStriped, 1 },
        { Layers.RedStriped, 1 },
        { Layers.GreenStriped, 1 },
        { Layers.OrangeStriped, 1 },
        { Layers.PinkStriped, 1 },
        { Layers.BrownStriped, 2 },
        { Layers.PurpleStriped, 1 },
        { Layers.SilverStriped, 2 },
        { Layers.BlackStriped, 2 },
        { Layers.BlueDiamond, 4 },
        { Layers.RedDiamond, 4 },
        { Layers.GreenDiamond, 2 },
        { Layers.BlackDiamond, 2 },
        { Layers.BlueDiamondStriped, 4 },
        { Layers.RedDiamondStriped,  4},
        { Layers.GreenDiamondStriped, 2 },
        { Layers.BlackDiamondStriped, 2 },
        { Layers.GiantDiamond, 5 },
    };
    internal static Dictionary<Layers, float> _layerHealths = new Dictionary<Layers, float>()
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
        { Layers.WhiteStriped, 2f },
        { Layers.YellowStriped, 4f },
        { Layers.BlueStriped, 4f },
        { Layers.RedStriped, 4f },
        { Layers.GreenStriped, 4f },
        { Layers.OrangeStriped, 8f },
        { Layers.PinkStriped, 8f },
        { Layers.BrownStriped, 12f },
        { Layers.PurpleStriped, 25f },
        { Layers.SilverStriped, 5f },
        { Layers.BlackStriped, 50f },
        { Layers.BlueDiamond, 150f },
        { Layers.RedDiamond, 300f },
        { Layers.GreenDiamond, 500f },
        { Layers.BlackDiamond, 1000f },
        { Layers.BlueDiamondStriped, 300f },
        { Layers.RedDiamondStriped,  600f},
        { Layers.GreenDiamondStriped, 1250f },
        { Layers.BlackDiamondStriped, 2500f },
        { Layers.GiantDiamond, 10000f },
    };
    internal static Dictionary<Layers, float> _layerSpeeds = new Dictionary<Layers, float>()
    {
        { Layers.White, 0.5f },
        { Layers.Yellow, 0.75f },
        { Layers.Blue, 1f },
        { Layers.Red, 1.5f },
        { Layers.Green, 2f },
        { Layers.Orange, 2.5f },
        { Layers.Pink, 3f },
        { Layers.Brown, 4f },
        { Layers.Purple, 5f },
        { Layers.Silver, 1f },
        { Layers.Black, 2f },
        { Layers.WhiteStriped, 0.5f },
        { Layers.YellowStriped, 0.75f },
        { Layers.BlueStriped, 1f },
        { Layers.RedStriped, 1.5f },
        { Layers.GreenStriped, 2f },
        { Layers.OrangeStriped, 2.5f },
        { Layers.PinkStriped, 3f },
        { Layers.BrownStriped, 4f },
        { Layers.PurpleStriped, 5f },
        { Layers.SilverStriped, 1f },
        { Layers.BlackStriped, 2f },
        { Layers.BlueDiamond, 2f },
        { Layers.RedDiamond, 1.5f },
        { Layers.GreenDiamond, 1f },
        { Layers.BlackDiamond, 0.5f },
        { Layers.BlueDiamondStriped, 2f },
        { Layers.RedDiamondStriped,  1.5f},
        { Layers.GreenDiamondStriped, 1f },
        { Layers.BlackDiamondStriped, 0.5f },
        { Layers.GiantDiamond, 0.25f },
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
        BlueDiamond = 23,
        RedDiamond = 24,
        GreenDiamond = 25,
        BlackDiamond = 26,
        BlueDiamondStriped = 27,
        RedDiamondStriped = 28,
        GreenDiamondStriped = 29,
        BlackDiamondStriped = 30,
        GiantDiamond = 31,
    }
    public static int GetNumLives(Layers layer)
    {
        int sum = 0;
        for(int i = (int)layer; i > 0; i--)
        {
            sum += (int)_layerHealths[(Layers)i];
        }
        return sum;
    }

}
    

