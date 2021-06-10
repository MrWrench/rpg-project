public class FireDebuff : BaseGaugeStatusFX
{
  public FireDebuff(Character target) : base(target) { }

  protected override void OnStart()
  {
    var exploded = TryExplode();
    
    if(exploded)
      return;

    target.onGaugeTriggered += gauge => TryExplode();
  }

  private bool TryExplode()
  {
    var gauges = target.GetGauges();
    var count = gauges.Count;
    
    var totalDamage = 0f;
    for (int i = 0; i < count; i++)
    {
      var gauge = gauges[i];
      if (gauge is StatusGauge status_gauge && status_gauge.triggered && status_gauge.statusType != owningGauge.statusType)
      {
        totalDamage += status_gauge.damage * status_gauge.gauge;
      }
    }

    if (totalDamage > 0)
    {
      for (int i = 0; i < count; i++) 
        gauges[i].Clear();
      totalDamage += damage * owningGauge.gauge;
      target.ApplyDamage(new DamageInfo{healthAmount = totalDamage, type = EnumDamageType.ELEMENTAL});
      return true;
    }

    return false;
  }
}