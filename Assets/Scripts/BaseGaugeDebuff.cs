public abstract class BaseGaugeDebuff : BaseDebuff
{
  protected float strength => gauge.strength;
  protected float damage => gauge.damage;
  protected float decayRate => gauge.baseDecayRate;
  
  private readonly BaseGauge gauge;
  
  protected BaseGaugeDebuff(Character target, BaseGauge gauge) : base(target)
  {
    this.gauge = gauge;
  }
}