public abstract class BaseGaugeStatusFX : BaseStatusFX
{
  public StatusGauge owningGauge;
  public float strength => owningGauge.strength;
  public float damage => owningGauge.damage;
  public float decayRate => owningGauge.baseDecayRate;
  public EnumStatusType statusType => owningGauge.statusType;
  
  protected BaseGaugeStatusFX(Character target) : base(target) { }
}