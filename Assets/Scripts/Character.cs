using System;
using System.Collections.Generic;
using StatusFX;
using StatusFX.Elemental;
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
		SetupStatuses();
	}

	private void SetupStatuses()
	{
		StatusEffects.Add(new FireStatus(this));
		StatusEffects.Add(new CryoStatus(this));
		StatusEffects.Add(new HydroStatus(this));
		StatusEffects.Add(new ElectroStatus(this));
		StatusEffects.Add(new PoisonStatus(this));
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