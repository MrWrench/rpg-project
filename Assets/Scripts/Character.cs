using System;
using System.Collections.Generic;
using StatusFX;
using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField] public Stats Stats = new Stats();
	public UnitTeam Team { get; set; }
	public readonly StatusEffects StatusEffects = new StatusEffects();
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
		StatusEffects.Tick();
	}
}