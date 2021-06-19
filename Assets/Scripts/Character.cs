using System;
using GameDebug;
using JetBrains.Annotations;
using StatusFX;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Character : MonoBehaviour, ICombatUnit
{
	[SerializeField] public PersistentStats Stats = new PersistentStats();

	public IStatusCollection StatusFX { get; } = new StatusCollection();

	public UnitTeam Team => _team;
	[SerializeField] private UnitTeam _team;

	#region Stats

	public float Health { get; private set; }
	public float Poise { get; private set; }

	public float DebuffDurationMult { get; set; }
	public float PoiseDamageDebuff { get; set; }

	#endregion

	public event IDamageable.ApplyDamageDelegate OnAppliedDamage;

	private Transform _transform;

	private void Awake()
	{
		_transform = GetComponent<Transform>();
	}

	private void Start()
	{
		Health = Stats.MaxHealth;
		Poise = Stats.MaxPoise;
		CharacterDebug.InvokeSpawn(this);
	}
	
	public void ApplyStatusEffect(StatusEffectType statusEffectType, StatusEffectInfo effectInfo, float factor = 1)
	{
		if (factor <= 0) throw new ArgumentOutOfRangeException(nameof(factor));

		if (!StatusFX.HasStatusEffectImplemented(statusEffectType))
		{
			var newStatus = StatusEffects.GetDefault(statusEffectType);
			ImplementStatusEffect(newStatus);
		}

		if (!(StatusFX.GetStatusEffectInfo(statusEffectType) is IGaugeStatusEffect status))
			throw new InvalidOperationException($"Status of type {statusEffectType} is not {nameof(IGaugeStatusEffect)}");
		status.Add(effectInfo, factor);
	}
	
	public void ClearStatusEffect(StatusEffectType statusEffectType)
	{
		if(!StatusFX.HasStatusEffectImplemented(statusEffectType))
			return;
    		
		if (!(StatusFX.GetStatusEffectInfo(statusEffectType) is IGaugeStatusEffect status))
			throw new InvalidOperationException($"Status of type {statusEffectType} is not {nameof(IGaugeStatusEffect)}");
    		
		status.Clear();
	}

	public void ImplementStatusEffect([NotNull] IStatusEffect statusEffect)
	{
		if (statusEffect == null) throw new ArgumentNullException(nameof(statusEffect));
		if(StatusFX.HasStatusEffectImplemented(statusEffect.EffectType)) throw new ArgumentException(nameof(statusEffect));
		
		StatusFX.AddStatusEffect(statusEffect);
		statusEffect.LinkNewTarget(this);
	}

	private void UnimplementStatusEffect([NotNull] IStatusEffect statusEffect)
	{
		if (statusEffect == null) throw new ArgumentNullException(nameof(statusEffect));
		if(!StatusFX.HasStatusEffectImplemented(statusEffect.EffectType)) throw new ArgumentException(nameof(statusEffect));
		StatusFX.RemoveStatusEffect(statusEffect);
		statusEffect.UnlinkCurrentTarget();
	}

	public void ReimplementStatusEffect([NotNull] IStatusEffect statusEffect)
	{
		if (statusEffect == null) throw new ArgumentNullException(nameof(statusEffect));
		var existingStatusEffect = StatusFX.FindStatusEffect(statusEffect.EffectType);
		if (existingStatusEffect != null) 
			UnimplementStatusEffect(existingStatusEffect);
		ImplementStatusEffect(statusEffect);
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

	public event IDamageable.TakeDamageDelegate OnTakeDamage;

	public void TakeDamage(DamageInfo info, float factor = 1)
	{
		OnTakeDamage?.Invoke(info, factor);
		ApplyDamage(info, factor);
	}

	private void ApplyDamage(DamageInfo info, float factor)
	{
		var healthAmount = info.HealthAmount * factor;
		Health -= healthAmount;
		var poiseAmount = (info.PoiseAmount + PoiseDamageDebuff) * factor;
		Poise -= poiseAmount;
		if (Poise <= 0)
			PoiseBreak();

		var appliedDamage = new DamageInfo(info.Type, healthAmount, poiseAmount);
		OnAppliedDamage?.Invoke(appliedDamage);
	}

	private void PoiseBreak()
	{
		Poise = Stats.MaxPoise;
	}

	public IObservable<Unit> GetUpdateObservable() => this.UpdateAsObservable();
	public Transform GetTransform() => _transform;
}