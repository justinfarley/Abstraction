public class Slowed : Debuff
{
    //TODO: visuals for this
    private float _originalSpeed;
    private void Start()
    {
        _lifeTime = 6.5f;
        _originalSpeed = _shape.Properties._moveSpeed;
        _shape.Properties._moveSpeed *= GameUtils.GLOBAL_SLOWED_DEBUFF_SPEED_REDUCTION_MULTIPLIER;
    }
    private void OnDestroy()
    {
        _shape.Properties._moveSpeed = _originalSpeed;
    }
}
