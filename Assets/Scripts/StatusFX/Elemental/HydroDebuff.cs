using JetBrains.Annotations;

namespace StatusFX
{
  [DefaultStatusFX(EnumStatusType.HYDRO)]
  public sealed class HydroDebuff : BaseGaugeStatusFX
  {
    private const float DEBUFF_AMOUNT = 0.5f;
    public override EnumStatusType statusType => EnumStatusType.HYDRO;

    public HydroDebuff([NotNull] Character target) : base(target) { }

    private float appliedAmount;

    protected override void OnStart()
    {
      appliedAmount = DEBUFF_AMOUNT * strength;
      target.debuffDurationMult += appliedAmount;
    }

    protected override void OnStop()
    {
      target.debuffDurationMult -= appliedAmount;
      appliedAmount = 0;
    }
  }
}