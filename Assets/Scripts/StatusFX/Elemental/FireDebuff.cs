﻿using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace StatusFX.Elemental
{
	[CreateAssetMenu(fileName = "Fire Debuff", menuName = "StatusFX/Fire Debuff", order = 2)]
	internal sealed class FireDebuff : ElementalDebuff, ISpecialEventStatusEffect<Unit>
	{
		[SerializeField] private float _explosionRadius = 5;
		[SerializeField] private float _statusSpreadMult = 0.4f;
		[SerializeField] private float _explosionDamageMult = 0.4f;
		[SerializeField] private float _explosionStrengthMult = 0.4f;
		[SerializeField] private float _explosionPoiseDamage = 40;
		[SerializeField] private float _hydroStrengthMult = 0.5f;

		private float ExplosionRadius => _explosionRadius;
		private float StatusSpreadMult => _statusSpreadMult;
		private float ExplosionDamageMult => _explosionDamageMult;
		private float ExplosionStrengthMult => _explosionStrengthMult;
		private float ExplosionPoiseDamage => _explosionPoiseDamage;
		private float HydroStrengthMult => _hydroStrengthMult;

		private readonly Subject<Unit> _onSpecialEvent = new Subject<Unit>();

		protected override void OnStart()
		{
			if (!TryExplode())
				Target.StatusFX.OnStatusEffectStarted += OnStatusEffectStarted;
		}

		protected override void OnUpdate()
		{
			if (!IsStarted)
				return;

			Target.TakeDamage(new DamageInfo(DamageType.Elemental, Damage * BaseDecayRate), Time.deltaTime);
		}

		protected override void OnStop()
		{
			Target.StatusFX.OnStatusEffectStarted -= OnStatusEffectStarted;
		}

		private void OnStatusEffectStarted(IStatusEffect statusEffect)
		{
			TryExplode();
		}

		private bool TryExplode()
		{
			if (!IsStarted)
				return false;

			var gauges = Target.StatusFX.AsEnumerable().OfType<IGaugeStatusEffect>().ToArray();
			var count = gauges.Length;

			var electroStrength = 0f;
			var cryoStrength = 0f;
			var hydroStrength = 0f;
			var totalAmount = Amount;
			var totalStrength = Strength;
			var statusCount = 1;

			var totalDamage = 0f;
			for (int i = 0; i < count; i++)
			{
				var status = gauges[i];
				if (status.IsStarted && status.EffectType != EffectType)
				{
					totalDamage += status.Damage * status.Amount * Strength;
					totalStrength += status.Strength;
					totalAmount += status.Amount;
					statusCount++;

					switch (status.EffectType)
					{
						case StatusEffectType.Electro:
							electroStrength = status.Strength;
							break;
						case StatusEffectType.Hydro:
							hydroStrength = status.Strength;
							break;
						case StatusEffectType.Cryo:
							cryoStrength = status.Strength;
							break;
					}
				}
			}

			var poiseDamage = ExplosionPoiseDamage * cryoStrength;

			if (totalDamage > 0)
			{
				totalDamage += Damage * Amount * Strength; // Прибавляем сам огонь
				totalDamage += totalDamage * hydroStrength * HydroStrengthMult; // Прибавляем бонус от воды

				for (int i = 0; i < count; i++)
					gauges[i].Clear();

				Target.TakeDamage(new DamageInfo(DamageType.Elemental, totalDamage, poiseDamage));

				if (electroStrength > 0)
				{
					ExplodeAoe(totalDamage, totalAmount, totalStrength, statusCount, poiseDamage);
				}

				return true;
			}

			return false;
		}

		private void ExplodeAoe(float totalDamage, float totalAmount, float totalStrength, int statusCount,
			float poiseDamage)
		{
			var explosionDamage = totalDamage * ExplosionDamageMult;
			var statusAmount = Mathf.Min(totalAmount * StatusSpreadMult, 1);
			var explosionStength = totalStrength / statusCount * ExplosionStrengthMult;

			var victims = StatusEffectsQueries.FindFriendsInSphere(Target, Target.GetTransform().position, ExplosionRadius);

			if (victims.Count > 0)
			{
				foreach (var victim in victims)
				{
					victim.TakeDamage(new DamageInfo(DamageType.Elemental, explosionDamage, poiseDamage));
					victim.ApplyStatusEffect(StatusEffectType.Fire,
						new StatusEffectInfo(statusAmount, explosionDamage, explosionStength));
				}
			}
		}

		public IObservable<Unit> OnSpecialEventAsObservable()
		{
			return _onSpecialEvent;
		}
	}
}