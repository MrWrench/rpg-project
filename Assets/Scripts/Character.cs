using System;
using System.Collections;
using System.Collections.Generic;
using StatusFX;
using UnityEngine;

public class Character : MonoBehaviour
{
  [SerializeField]
  public PersistentStats stats = new PersistentStats();

  #region Stats

  public float health { get; private set; }
  public float poise { get; private set; }
  
  public float debuffDurationMult = 0;
  public float poiseDamageDebuff = 0;

  #endregion

  public event Action<BaseGaugeStatusFX>? onGaugeTriggered; 
  public event Action<DamageInfo>? onTakeDamage; 

  private readonly List<BaseGaugeStatusFX> gaugeList = new List<BaseGaugeStatusFX>();
  private readonly Dictionary<EnumStatusType, BaseGaugeStatusFX> gaugeDict = new Dictionary<EnumStatusType, BaseGaugeStatusFX>();

  // Start is called before the first frame update
  void Start()
  {
    health = stats.maxHealth;
    poise = stats.maxPoise;
  }

  private void ImplementGauge(BaseGaugeStatusFX status_fx)
  {
    gaugeList.Add(status_fx);
    gaugeDict.Add(status_fx.statusType, status_fx);
    status_fx.onTriggered += base_gauge => onGaugeTriggered?.Invoke(base_gauge);
  }

  public void ApplyStatus(AddStatusInfo info)
  {
    var status = info.status;
    if(gaugeDict.ContainsKey(status))
      ImplementGauge(DefaultStatusGaugePool.Instantiate(status, this));
    
    gaugeDict[status].Add(info);
  }

  // Update is called once per frame
  private void Update()
  {
    foreach (var gauge in gaugeList) 
      gauge.Update();
  }

  public void ApplyDamage(DamageInfo info, float factor = 1)
  {
    health -= info.healthAmount * factor;
    poise -= (info.poiseAmount + poiseDamageDebuff) * factor;
    if(poise <= 0)
      PoiseBreak();
  }

  private void PoiseBreak()
  {
    poise = stats.maxPoise;
  }

  public IReadOnlyList<BaseGaugeStatusFX> GetGauges() => gaugeList;
}