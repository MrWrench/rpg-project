public class StatusGauge : BaseGauge
{
  public readonly EnumStatusType statusType;
  private readonly BaseStatusFX status_fx;

  public StatusGauge(Character owner, EnumStatusType statusType, BaseStatusFX status_fx) : base(owner)
  {
    this.statusType = statusType;
    this.status_fx = status_fx;
    if (status_fx is BaseGaugeStatusFX gauge_status_fx) 
      gauge_status_fx.owningGauge = this;
  }

  protected override void OnTrigger()
  {
    status_fx.Start();
  }

  protected override void OnExhaust()
  {
    status_fx.Stop();
  }

  protected override void OnUpdate()
  {
    status_fx.Update();
  }
}