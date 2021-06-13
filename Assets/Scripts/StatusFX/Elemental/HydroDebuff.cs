using JetBrains.Annotations;

namespace StatusFX
{
  [DefaultStatusFX(EnumStatusType.HYDRO)]
  internal sealed class HydroDebuff : GaugeStatusEffect
  {
    private const float DEBUFF_AMOUNT = 0.5f;
    public override EnumStatusType type => EnumStatusType.HYDRO;
    public override bool isDebuff => true;

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