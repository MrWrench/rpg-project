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
  
  public float debuffDurationMult;
  public float poiseDamageDebuff;

  #endregion

  public event Action<IStatusEffect>? onStatusEffectStarted; 
  
  public delegate void TakeDamageDelegate(DamageInfo info, float factor);
  public event TakeDamageDelegate? onTakeDamage; 

  private readonly List<BaseGaugeStatusFX> statusList = new List<BaseGaugeStatusFX>();
  private readonly Dictionary<EnumStatusType, BaseGaugeStatusFX> statusDict = new Dictionary<EnumStatusType, BaseGaugeStatusFX>();

  private void Start()
  {
    health = stats.maxHealth;
    poise = stats.maxPoise;
    OnSpawn?.Invoke(this);
  }

  private void ImplementGauge(BaseGaugeStatusFX status_fx)
  {
    statusList.Add(status_fx);
    statusDict.Add(status_fx.statusType, status_fx);
    status_fx.onStarted += base_gauge => onStatusEffectStarted?.Invoke(base_gauge);
  }

  public void ApplyStatus(EnumStatusType statusType, AddStatusInfo info, float factor = 1)
  {
    info.amount *= factor;
    if(!statusDict.ContainsKey(statusType))
      ImplementGauge(DefaultStatusGaugePool.Instantiate(statusType, this));
    
    statusDict[statusType].Add(info);
  }

  // Update is called once per frame
  private void Update()
  {
    foreach (var gauge in statusList) 
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

  public IReadOnlyList<BaseGaugeStatusFX> GetGauges() => statusList;

  public void ClearStatus(EnumStatusType statusType)
  {
    if (statusDict.TryGetValue(statusType, out var statusFX))
    {
      statusFX.Clear();
    }
  }
}