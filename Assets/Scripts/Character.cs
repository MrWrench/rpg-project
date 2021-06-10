using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
  public PersistentStats stats;

  #region Stats

  public float health;
  public float poise;

  #endregion

  private List<BaseGauge> gaugeList = new List<BaseGauge>();
  private Dictionary<EnumDebuffType, BaseGauge> gaugeDict = new Dictionary<EnumDebuffType, BaseGauge>();
  

  // Start is called before the first frame update
  void Start()
  {
    health = stats.maxHealth;
    poise = stats.maxPoise;
  }

  void AddGauge(EnumDebuffType type, BaseStatusFX status_fx)
  {
    var gauge = new StatusGauge(this, status_fx);
    gaugeList.Add(gauge);
    gaugeDict.Add(type, gauge);
  }

  // Update is called once per frame
  void Update()
  {
  }
}