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

  public EnumTeam team => _team;
  [SerializeField] private EnumTeam _team;

  #region Stats

  public float health { get; private set; }
  public float poise { get; private set; }
  
  public float debuffDurationMult { get; set; }
  public float poiseDamageDebuff { get; set; }

  #endregion

  public event Action<IStatusEffect>? onStatusEffectStarted; 
  
  public delegate void TakeDamageDelegate(DamageInfo info, float factor);
  public event TakeDamageDelegate? onTakeDamage;
  
  public delegate void ApplyDamageDelegate(DamageInfo info);
  public event ApplyDamageDelegate? onAppliedDamage;

  private readonly List<BaseGaugeStatusFX> statusFXList = new List<BaseGaugeStatusFX>();
  private readonly Dictionary<EnumStatusType, BaseGaugeStatusFX> statusFXDict = new Dictionary<EnumStatusType, BaseGaugeStatusFX>();

  private void Start()
  {
    health = stats.maxHealth;
    poise = stats.maxPoise;
    OnSpawn?.Invoke(this);
  }

  private void ImplementGauge(BaseGaugeStatusFX status_fx)
  {
    statusFXList.Add(status_fx);
    statusFXDict.Add(status_fx.statusType, status_fx);
    status_fx.onStarted += base_gauge => onStatusEffectStarted?.Invoke(base_gauge);
  }

  public void ApplyStatus(EnumStatusType statusType, StatusEffectInfo effectInfo, float factor = 1)
  {
    if(!statusFXDict.ContainsKey(statusType))
      ImplementGauge(DefaultStatusGaugePool.Instantiate(statusType, this));
    
    statusFXDict[statusType].Add(effectInfo, factor);
  }

  // Update is called once per frame
  private void Update()
  {
    foreach (var gauge in statusFXList) 
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
    var healthAmount = info.healthAmount * factor;
    health -= healthAmount;
    var poiseAmount = (info.poiseAmount + poiseDamageDebuff) * factor;
    poise -= poiseAmount;
    if (poise <= 0)
      PoiseBreak();

    var appliedDamage = new DamageInfo(info.type, healthAmount, poiseAmount);
    onAppliedDamage?.Invoke(appliedDamage);
  }

  private void PoiseBreak()
  {
    poise = stats.maxPoise;
  }

  public IReadOnlyList<BaseGaugeStatusFX> GetStatusEffects() => statusFXList;

  public void ClearStatus(EnumStatusType statusType)
  {
    if (statusFXDict.TryGetValue(statusType, out var statusFX))
    {
      statusFX.Clear();
    }
  }
}