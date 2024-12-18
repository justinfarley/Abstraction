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
        { Layers.Orange, 2 },
        { Layers.Pink, 1 },
        { Layers.Purple, 1 },
        { Layers.Silver, 1 },
        { Layers.Black, 2 },
        { Layers.Brown, 2 },
        { Layers.WhiteStriped, 1 },
        { Layers.YellowStriped, 1 },
        { Layers.BlueStriped, 1 },
        { Layers.RedStriped, 1 },
        { Layers.GreenStriped, 1 },
        { Layers.OrangeStriped, 2 },
        { Layers.PinkStriped, 1 },
        { Layers.BrownStriped, 2 },
        { Layers.PurpleStriped, 1 },
        { Layers.SilverStriped, 1 },
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
        { Layers.Yellow, 1f },
        { Layers.Blue, 1f },
        { Layers.Red, 1f },
        { Layers.Green, 1f },
        { Layers.Orange, 1f },
        { Layers.Pink, 1f },
        { Layers.Brown, 20f },
        { Layers.Purple, 1f },
        { Layers.Silver, 1f },
        { Layers.Black, 1f },
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
        { Layers.Blue, 0.75f },
        { Layers.Red, 0.873f },
        { Layers.Green, 1f },
        { Layers.Orange, 1.25f },
        { Layers.Pink, 2f },
        { Layers.Brown, 1.5f },
        { Layers.Purple, 1.5f },
        { Layers.Silver, 0.75f },
        { Layers.Black, 1.125f },
        { Layers.WhiteStriped, 0.25f },
        { Layers.YellowStriped, 0.375f },
        { Layers.BlueStriped, 0.5f },
        { Layers.RedStriped, 0.75f },
        { Layers.GreenStriped, 1f },
        { Layers.OrangeStriped, 1.25f },
        { Layers.PinkStriped, 1.5f },
        { Layers.BrownStriped, 2f },
        { Layers.PurpleStriped, 2.5f },
        { Layers.SilverStriped, 0.5f },
        { Layers.BlackStriped, 1f },
        { Layers.BlueDiamond, 1f },
        { Layers.RedDiamond, 0.75f },
        { Layers.GreenDiamond, 0.5f },
        { Layers.BlackDiamond, 0.25f },
        { Layers.BlueDiamondStriped, 1f },
        { Layers.RedDiamondStriped,  0.75f},
        { Layers.GreenDiamondStriped, 0.5f },
        { Layers.BlackDiamondStriped, 0.25f },
        { Layers.GiantDiamond, 0.125f },
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
        Purple = 8,
        Silver = 9,
        Black = 10,
        Brown = 11,
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
    

