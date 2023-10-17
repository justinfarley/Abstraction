public class Level1 : AbstractLevel
{
    public override void Start()
    {
        base.Start();
    }
    public override void Awake()
    {
        base.Awake();
        _levelProperties = new LevelProperties(LevelProperties.Mode.Baby);
        _levelProperties._name = "Level 1";

    }
}
