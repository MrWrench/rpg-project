namespace StatusFX
{
  [DefaultStatusFX(EnumStatusType.FIRE)]
  public sealed class FireDebuff : BaseGaugeStatusFX
  {
    public override EnumStatusType statusType => EnumStatusType.FIRE;

    public FireDebuff(Character target) : base(target) { }

    protected override void OnStart()
    {
      var exploded = TryExplode();
    
      if(exploded)
        return;

      target.onGaugeTriggered += status => TryExplode();
    }

    protected override void OnUpdate()
    {
      if(!started)
        return;
    
      target.ApplyDamage(new DamageInfo(EnumDamageType.ELEMENTAL, damage * baseDecayRate));
    }

    protected override void OnStop()
    {
      target.onGaugeTriggered -= status => TryExplode();
    }

    private bool TryExplode()
    {
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
        for (int i = 0; i < count; i++) 
          gauges[i].Clear();
        totalDamage += damage * amount;
        target.ApplyDamage(new DamageInfo(EnumDamageType.ELEMENTAL, totalDamage));
        return true;
      }

      return false;
    }
  }
}