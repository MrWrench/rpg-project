using JetBrains.Annotations;

namespace StatusFX
{
  [DefaultStatusFX(EnumStatusType.CRYO)]
  public sealed class CryoDebuff: BaseGaugeStatusFX
  {
    private const float POISE_DEBUFF = 10;
    public override EnumStatusType statusType => EnumStatusType.CRYO;
    
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