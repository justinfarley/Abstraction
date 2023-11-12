using System;

[Serializable]
public class RoundsData
{
    private const AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant CAMO = AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant.Camo;
    private const AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant REGEN = AbstractShapeEnemy.ShapeEnemyProperties.ShapeVariant.Regen;
    public Round[] _rounds =
        {
        //Round 1
        new Round(
      new Round.Wave[]{
                Round.MakeWave(10, 1f, Layer.Layers.White),
            },Round.WaveSpawnType.Seperate
        ),
        //Round 2: Introduce Yellows
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(3, 2f, Layer.Layers.Yellow),
                 Round.MakeWave(10, 0.75f, Layer.Layers.White),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 3
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(5, 0.6f, Layer.Layers.Yellow),
                 Round.MakeWave(20, 0.4f, Layer.Layers.White),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 4: introduce Blues
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(5, 1f, Layer.Layers.Blue),
                 Round.MakeWave(10, 0.5f, Layer.Layers.Yellow),
            },Round.WaveSpawnType.Together
            ),
        //Round 5: 
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(10, 1f, Layer.Layers.Blue),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 6:
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(10, 2f, Layer.Layers.Blue),
                 Round.MakeWave(50, 0.25f, Layer.Layers.White),
            },Round.WaveSpawnType.Together
            ),
        //Round 7:
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(5, 5f, Layer.Layers.Red),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 8:
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(5, 5f, Layer.Layers.Red),
                 Round.MakeWave(10, 2.5f, Layer.Layers.Red),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 9:
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(5, 1f, Layer.Layers.Green),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 10:
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(5, 2f, Layer.Layers.Green),
                 Round.MakeWave(10, 1f, Layer.Layers.Red),
                 Round.MakeWave(25, 0.5f, Layer.Layers.White),
            },Round.WaveSpawnType.Together
            ),
        //Round 11:
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(50, 0.25f, Layer.Layers.Red),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 12:
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(50, 0.05f, Layer.Layers.White),
                 Round.MakeWave(10, 1f, Layer.Layers.Green),
                 Round.MakeWave(20, 0.25f, Layer.Layers.Blue),
            },Round.WaveSpawnType.Together
            ),
        //Round 13:
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(200, 0.05f, Layer.Layers.White),
                 Round.MakeWave(20, 0.25f, Layer.Layers.Orange),
            },Round.WaveSpawnType.Together
            ),
        //Round 14:
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(5, 0.5f, Layer.Layers.Pink),
                 Round.MakeWave(10, 0.25f, Layer.Layers.Pink),
                 Round.MakeWave(20, 0.1f, Layer.Layers.Pink),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 15:
        new Round(
        new Round.Wave[]{
                 Round.MakeWave(1, 0f, Layer.Layers.Red, CAMO),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 16:
        new Round(
        new Round.Wave[]{
                Round.MakeWave(25, 0.1f, Layer.Layers.Green),
                Round.MakeWave(25, 0.1f, Layer.Layers.Orange),
                Round.MakeWave(25, 0.1f, Layer.Layers.Pink),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 17:
        new Round(
        new Round.Wave[]{
                Round.MakeWave(50, 0.25f, Layer.Layers.Green),
                Round.MakeWave(10, 1f, Layer.Layers.Brown),
            },Round.WaveSpawnType.Together
            ),
        //Round 18:
        new Round(
        new Round.Wave[]{
                Round.MakeWave(50, 0.25f, Layer.Layers.Green, CAMO),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 19:
        new Round(
        new Round.Wave[]{
                Round.MakeWave(50, 0.25f, Layer.Layers.Green, REGEN),
                Round.MakeWave(25, 0.5f, Layer.Layers.Blue, REGEN),
                Round.MakeWave(10, 1f, Layer.Layers.White, REGEN),
                Round.MakeWave(20, 1f, Layer.Layers.Red, CAMO),
            },2
            ),
        //Round 20:
        new Round(
        new Round.Wave[]{
                Round.MakeWave(1, 0.5f, Layer.Layers.White),
                Round.MakeWave(2, 0.5f, Layer.Layers.Yellow),
                Round.MakeWave(4, 0.5f, Layer.Layers.Blue),
                Round.MakeWave(8, 0.25f, Layer.Layers.Red),
                Round.MakeWave(16, 0.25f, Layer.Layers.Green),
                Round.MakeWave(32, 0.2f, Layer.Layers.Orange),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 21:
        new Round(
        new Round.Wave[]{
                Round.MakeWave(100, 0.375f, Layer.Layers.Orange, REGEN),
            },Round.WaveSpawnType.Seperate
            ),
        //Round 22: TEST ROUND
        new Round(
        new Round.Wave[]{
                Round.MakeWave(1, 1f, Layer.Layers.Green, REGEN),
            },Round.WaveSpawnType.Seperate
            ),
        };
}
