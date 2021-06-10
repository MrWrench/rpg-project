using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
  public PersistentStats stats;

  #region Stats

  public float health { get; private set; }
  public float poise { get; private set; }
  
  public float debuffDurationMult = 0;
  public float poiseDamageDebuff = 0;

  #endregion

  public event Action<BaseGauge> onGaugeTriggered; 

  private List<BaseGauge> gaugeList = new List<BaseGauge>();
  private Dictionary<EnumStatusType, BaseGauge> gaugeDict = new Dictionary<EnumStatusType, BaseGauge>();
  

  // Start is called before the first frame update
  void Start()
  {
    health = stats.maxHealth;
    poise = stats.maxPoise;
  }

  void AddGauge(EnumStatusType type, BaseStatusFX status_fx)
  {
    var gauge = new StatusGauge(this, type, status_fx);
    gaugeList.Add(gauge);
    gaugeDict.Add(type, gauge);
    gauge.onTriggered += base_gauge => onGaugeTriggered?.Invoke(base_gauge);
  }

  // Update is called once per frame
  void Update()
  {
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

  public IReadOnlyList<BaseGauge> GetGauges() => gaugeList;
}