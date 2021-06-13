using System;
using System.Collections;
using System.Collections.Generic;
using StatusFX;
using UnityEngine;

public class Character : MonoBehaviour
{
  public static event Action<Character>? OnSpawn;
  public static event Action? OnDestroyed;
  public static event Action<Character>? OnEnabled;
  public static event Action<Character>? OnDisabled;
  
  [SerializeField]
  public PersistentStats stats = new PersistentStats();

  #region Stats

  public float health { get; private set; }
  public float poise { get; private set; }
  
  public float debuffDurationMult = 0;
  public float poiseDamageDebuff = 0;

  #endregion

  public event Action<BaseGaugeStatusFX>? onGaugeTriggered; 
  
  public delegate void TakeDamageDelegate(DamageInfo info, float factor);
  public event TakeDamageDelegate? onTakeDamage; 

  private readonly List<BaseGaugeStatusFX> gaugeList = new List<BaseGaugeStatusFX>();
  private readonly Dictionary<EnumStatusType, BaseGaugeStatusFX> gaugeDict = new Dictionary<EnumStatusType, BaseGaugeStatusFX>();

  // Start is called before the first frame update
  void Start()
  {
    health = stats.maxHealth;
    poise = stats.maxPoise;
    OnSpawn?.Invoke(this);
  }

  private void ImplementGauge(BaseGaugeStatusFX status_fx)
  {
    gaugeList.Add(status_fx);
    gaugeDict.Add(status_fx.statusType, status_fx);
    status_fx.onTriggered += base_gauge => onGaugeTriggered?.Invoke(base_gauge);
  }

  public void ApplyStatus(AddStatusInfo info, float factor = 1)
  {
    info.amount *= factor;
    var status = info.status;
    if(!gaugeDict.ContainsKey(status))
      ImplementGauge(DefaultStatusGaugePool.Instantiate(status, this));
    
    gaugeDict[status].Add(info);
  }

  // Update is called once per frame
  private void Update()
  {
    foreach (var gauge in gaugeList) 
      gauge.Update();
  }

  private void OnDestroy()
  {
    OnDestroyed?.Invoke();
  }

  private void OnEnable()
  {
    OnEnabled?.Invoke(this);
  }

  private void OnDisable()
  {
    OnDisabled?.Invoke(this);
  }

  public void TakeDamage(DamageInfo info, float factor = 1)
  {
    onTakeDamage?.Invoke(info, factor);
    ApplyDamage(info, factor);
  }

  private void ApplyDamage(DamageInfo info, float factor)
  {
    health -= info.healthAmount * factor;
    poise -= (info.poiseAmount + poiseDamageDebuff) * factor;
    if (poise <= 0)
      PoiseBreak();
  }

  private void PoiseBreak()
  {
    poise = stats.maxPoise;
  }

  public IReadOnlyList<BaseGaugeStatusFX> GetGauges() => gaugeList;

  public void ClearStatus(EnumStatusType statusType)
  {
    if (gaugeDict.TryGetValue(statusType, out var statusFX))
    {
      statusFX.Clear();
    }
  }
}