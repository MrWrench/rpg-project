public class StatusGauge : BaseGauge
{
  private readonly BaseStatusFX status_fx;

  public StatusGauge(Character owner, BaseStatusFX status_fx) : base(owner)
  {
    this.status_fx = status_fx;
  }

  protected override void OnTrigger()
  {
    status_fx.Start();
  }

  protected override void OnExhaust()
  {
    status_fx.Stop();
  }
}