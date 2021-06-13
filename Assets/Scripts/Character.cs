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

	[SerializeField] public PersistentStats stats = new PersistentStats();

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

	private readonly List<IGaugeStatusEffect> gaugeStatusFXList = new List<IGaugeStatusEffect>();

	private readonly Dictionary<EnumStatusType, IGaugeStatusEffect> gaugeStatusFXDict =
		new Dictionary<EnumStatusType, IGaugeStatusEffect>();

	private void Start()
	{
		health = stats.maxHealth;
		poise = stats.maxPoise;
		OnSpawn?.Invoke(this);
	}

	private void ImplementStatusEffect(IStatusEffect statusEffect)
	{
		if (statusEffect is IGaugeStatusEffect gaugeStatusEffect)
		{
			gaugeStatusFXList.Add(gaugeStatusEffect);
			gaugeStatusFXDict.Add(statusEffect.type, gaugeStatusEffect);
		}

		statusEffect.onStarted += base_gauge => onStatusEffectStarted?.Invoke(base_gauge);
	}

	public void ApplyStatus(EnumStatusType statusType, StatusEffectInfo effectInfo, float factor = 1)
	{
		if (!gaugeStatusFXDict.ContainsKey(statusType))
			ImplementStatusEffect(DefaultStatusEffectPool.Instantiate(statusType, this));

		gaugeStatusFXDict[statusType].Add(effectInfo, factor);
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

	public IReadOnlyList<IGaugeStatusEffect> GetGaugeStatusFX() => gaugeStatusFXList;

	public IReadOnlyGaugeStatusEffect GetGaugeStatusEffect(EnumStatusType statusType)
	{
		if (gaugeStatusFXDict.TryGetValue(statusType, out var result))
			return result;

		return GaugeStatusEffect.GetEmpty(statusType);
	}

	public void ClearStatus(EnumStatusType statusType)
	{
		if (gaugeStatusFXDict.TryGetValue(statusType, out var statusFX))
		{
			statusFX.Clear();
		}
	}
}