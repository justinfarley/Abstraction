public class Stunned : Debuff
{
    //TODO: visuals for this
    private float _originalSpeed;
    private void Start()
    {
        _lifeTime = 0.5f;
        _originalSpeed = _shape.Properties._moveSpeed;
        _shape.Properties._moveSpeed = 0;
    }
    private void OnDestroy()
    {
        _shape.Properties._moveSpeed = _originalSpeed;
    }
}
