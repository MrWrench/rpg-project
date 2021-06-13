using JetBrains.Annotations;

namespace StatusFX
{
  [DefaultStatusFX(EnumStatusType.CRYO)]
  internal sealed class CryoDebuff: GaugeStatusEffect
  {
    private const float POISE_DEBUFF = 10;
    public override EnumStatusType type => EnumStatusType.CRYO;
    public override bool isDebuff => true;

    private float appliedAmount;

    public CryoDebuff([NotNull] Character target) : base(target) { }

    protected override void OnStart()
    {
      appliedAmount = POISE_DEBUFF * strength;
      target.poiseDamageDebuff += appliedAmount;
    }

    protected override void OnStop()
    {
      target.poiseDamageDebuff -= appliedAmount;
      appliedAmount = 0;
    }
  }
}