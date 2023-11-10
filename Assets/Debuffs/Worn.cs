public class Worn : Debuff
{
    //TODO: visuals for this
    private float _originalDmgMult;
    private float _damageMultiplier;
    public void SetDamageMultiplier(float multiplier)
    {
        _damageMultiplier = multiplier;
    }
    private void Start()
    {
        _lifeTime = 4f;
        _originalDmgMult = _shape.Properties._localDamageTakenMultiplier;
        _shape.Properties._localDamageTakenMultiplier = _damageMultiplier; //2x damage until its destroyed
    }
    private void OnDestroy()
    {
        _shape.Properties._localDamageTakenMultiplier = _originalDmgMult;
    }
}
