using UnityEngine;

namespace StatusFX
{
  [DefaultStatusFX(EnumStatusType.FIRE)]
  public sealed class FireDebuff : BaseGaugeStatusFX
  {
    public override EnumStatusType statusType => EnumStatusType.FIRE;

    public FireDebuff(Character target) : base(target)
    {
      target.onGaugeTriggered += status => TryExplode();
    }

    protected override void OnStart()
    {
      TryExplode();
    }

    protected override void OnUpdate()
    {
      if(!started)
        return;
    
      target.TakeDamage(new DamageInfo(EnumDamageType.ELEMENTAL, damage * baseDecayRate), Time.deltaTime);
    }

    protected override void OnStop()
    {
    }

    private bool TryExplode()
    {
      if (!started)
        return false;
      
      var gauges = target.GetGauges();
      var count = gauges.Count;
    
      var totalDamage = 0f;
      for (int i = 0; i < count; i++)
      {
        var status = gauges[i];
        if (status.started && status.statusType != statusType)
        {
          totalDamage += damage * status.amount;
        }
      }

      if (totalDamage > 0)
      {
        totalDamage += damage * amount;
        for (int i = 0; i < count; i++) 
          gauges[i].Clear();
        target.TakeDamage(new DamageInfo(EnumDamageType.ELEMENTAL, totalDamage));
        return true;
      }

      return false;
    }
  }
}