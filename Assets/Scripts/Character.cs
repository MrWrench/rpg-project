using System;
using System.Collections.Generic;
using StatusFX;
using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField] public Stats Stats = new Stats();
	public readonly List<StatusEffect> StatusEffects = new List<StatusEffect>();
	public event Action<DamageInfo> OnDamageTaken; 

	private void Start()
	{
		Stats.Reset();
	}

	public void ApplyDamage(DamageInfo info)
	{
		Stats.Health -= info.HealthAmount * info.Multiplier;
		Stats.Poise -= (info.PoiseAmount + Stats.PoiseDamageDebuff) * info.Multiplier;
		OnDamageTaken?.Invoke(info);
	}

	private void Update()
	{
		foreach (var statusEffect in StatusEffects)
		{
			statusEffect.Tick();
		}
	}
}