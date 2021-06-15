using System;
using Debug;
using StatusFX;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Character : MonoBehaviour, ICombatUnit
{
	[SerializeField] public PersistentStats stats = new PersistentStats();

	public IStatusCollection statusFX { get; } = new StatusCollection();

	public EnumTeam team => _team;
	[SerializeField] private EnumTeam _team;

	#region Stats

	public float health { get; private set; }
	public float poise { get; private set; }

	public float debuffDurationMult { get; set; }
	public float poiseDamageDebuff { get; set; }

	#endregion

	public event IDamageable.ApplyDamageDelegate? onAppliedDamage;

	private void Start()
	{
		health = stats.maxHealth;
		poise = stats.maxPoise;
		CharacterDebug.InvokeSpawn(this);
	}
	
	public void ApplyStatus(EnumStatusType statusType, StatusEffectInfo effectInfo, float factor = 1)
	{
		if (factor <= 0) throw new ArgumentOutOfRangeException(nameof(factor));

		if (!statusFX.HasStatusEffectImplemented(statusType))
		{
			var newStatus = StatusEffect.GetDefault(statusType);
			newStatus.LinkNewTarget(this);
		}

		if (!(statusFX.GetStatusEffect(statusType) is IGaugeStatusEffect status))
			throw new InvalidOperationException($"Status of type {statusType} is not {nameof(IGaugeStatusEffect)}");
		status.Add(effectInfo, factor);
	}
	
	public void ClearStatus(EnumStatusType statusType)
	{
		if(!statusFX.HasStatusEffectImplemented(statusType))
			return;
    		
		if (!(statusFX.GetStatusEffect(statusType) is IGaugeStatusEffect status))
			throw new InvalidOperationException($"Status of type {statusType} is not {nameof(IGaugeStatusEffect)}");
    		
		status.Clear();
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

	public IObservable<Unit> GetUpdateObservable() => this.UpdateAsObservable();
}