public abstract class BaseGaugeStatusFX : BaseStatusFX
{
  public float strength;
  public float damage;
  public float decayRate;
  
  protected BaseGaugeStatusFX(Character target) : base(target) { }
}