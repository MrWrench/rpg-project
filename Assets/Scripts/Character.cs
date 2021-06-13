using System;
using System.Collections.Generic;
using Debug;
using JetBrains.Annotations;
using StatusFX;
using StatusFX.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Character : MonoBehaviour, ICombatUnit
{
	[SerializeField] public PersistentStats stats = new PersistentStats();

	public EnumTeam team => _team;
	[SerializeField] private EnumTeam _team;

	#region Stats

	public float health { get; private set; }
	public float poise { get; private set; }

	public float debuffDurationMult { get; set; }
	public float poiseDamageDebuff { get; set; }

	#endregion

	public event IDamageable.ApplyDamageDelegate? onAppliedDamage;

	private readonly List<IStatusEffect> statusFX = new List<IStatusEffect>();
	private readonly Dictionary<EnumStatusType, IStatusEffect> statusFXDict = new Dictionary<EnumStatusType, IStatusEffect>();

	private void Start()
	{
		health = stats.maxHealth;
		poise = stats.maxPoise;
		CharacterDebug.InvokeSpawn(this);
	}

	bool IStatusFXCarrier.HasStatusEffectImplemented(EnumStatusType statusType) => HasStatusEffectImplemented(statusType);
	private bool HasStatusEffectImplemented(EnumStatusType statusType) => statusFXDict.ContainsKey(statusType); 

	void IStatusFXCarrier.ImplementStatusEffect([NotNull] IStatusEffect statusEffect)
	{
		if (statusEffect == null) throw new ArgumentNullException(nameof(statusEffect));
		if (HasStatusEffectImplemented(statusEffect.type)) throw new ArgumentException(nameof(statusEffect));

		statusFX.Add(statusEffect);
		statusFXDict.Add(statusEffect.type, statusEffect);
		statusEffect.onStarted += OnStatusEffectStarted;
		statusEffect.onStoped += OnStatusEffectStoped;
	}

	void IStatusFXCarrier.UnimplementStatusEffect([NotNull] IStatusEffect statusEffect)
	{
		if (statusEffect == null) throw new ArgumentNullException(nameof(statusEffect));
		if (!HasStatusEffectImplemented(statusEffect.type)) throw new ArgumentException(nameof(statusEffect));

		statusEffect.onStarted -= OnStatusEffectStarted;
		statusEffect.onStoped -= OnStatusEffectStoped;
		statusFX.Remove(statusEffect);
		statusFXDict.Remove(statusEffect.type);
	}

	private void OnStatusEffectStarted(IStatusEffect statusEffect) => onStatusEffectStarted?.Invoke(statusEffect);
	private void OnStatusEffectStoped(IStatusEffect statusEffect) => onStatusEffectStoped?.Invoke(statusEffect);

	public void ApplyStatus(EnumStatusType statusType, StatusEffectInfo effectInfo, float factor = 1)
	{
		if (factor <= 0) throw new ArgumentOutOfRangeException(nameof(factor));

		if (!HasStatusEffectImplemented(statusType))
		{
			var newStatus = StatusEffect.GetDefault(statusType);
			newStatus.LinkNewTarget(this);
		}

		if (!(GetStatusEffect(statusType) is IGaugeStatusEffect status))
			throw new InvalidOperationException($"Status of type {statusType} is not {nameof(IGaugeStatusEffect)}");
		status.Add(effectInfo, factor);
	}

	private void OnDestroy()
	{
		CharacterDebug.InvokeDestroyed();
	}

	private void OnEnable()
	{
		CharacterDebug.InvokeEnabled(this);
	}

	private void OnDisable()
	{
		CharacterDebug.InvokeDisabled(this);
	}

	public event IDamageable.TakeDamageDelegate? onTakeDamage;

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

	public event IStatusFXCarrier.StatusEffectChangeDelegate? onStatusEffectStarted;
	public event IStatusFXCarrier.StatusEffectChangeDelegate? onStatusEffectStoped;

	public void ClearStatus(EnumStatusType statusType)
	{
		if(!HasStatusEffectImplemented(statusType))
			return;
		
		if (!(GetStatusEffect(statusType) is IGaugeStatusEffect status))
			throw new InvalidOperationException($"Status of type {statusType} is not {nameof(IGaugeStatusEffect)}");
		
		status.Clear();
	}

	public IEnumerable<IStatusEffect> GetStatusFX() => statusFX;

	public IReadOnlyStatusEffect GetStatusEffect(EnumStatusType type)
	{
		return HasStatusEffectImplemented(type) ? statusFXDict[type] : StatusEffect.GetEmpty(type);
	}

	public IObservable<Unit> GetUpdateObservable() => this.UpdateAsObservable();
}