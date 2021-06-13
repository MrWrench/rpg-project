using UnityEngine;

namespace StatusFX
{
  [DefaultStatusFX(EnumStatusType.POISON)]
  internal sealed class PoisonDebuff : GaugeStatusEffect
  {
    public override EnumStatusType type => EnumStatusType.POISON;
    public override bool isDebuff => true;

    public PoisonDebuff(Character target) : base(target) { }

    protected override void OnStart()
    {
    }

    protected override void OnUpdate()
    {
      if(!isStarted)
        return;
    
      target.TakeDamage(new DamageInfo(EnumDamageType.ELEMENTAL, damage * baseDecayRate), Time.deltaTime);
    }
  }
}