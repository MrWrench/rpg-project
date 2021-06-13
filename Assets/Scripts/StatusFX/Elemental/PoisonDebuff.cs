﻿using UnityEngine;

namespace StatusFX
{
  [DefaultStatusFX(EnumStatusType.POISON)]
  public sealed class PoisonDebuff : BaseGaugeStatusFX
  {
    public override EnumStatusType statusType => EnumStatusType.POISON;
    public override bool isDebuff => true;

    public PoisonDebuff(Character target) : base(target) { }

    protected override void OnStart()
    {
    }

    protected override void OnUpdate()
    {
      if(!started)
        return;
    
      target.TakeDamage(new DamageInfo(EnumDamageType.ELEMENTAL, damage * baseDecayRate), Time.deltaTime);
    }
  }
}