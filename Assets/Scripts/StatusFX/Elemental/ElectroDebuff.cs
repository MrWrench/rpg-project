using System.Linq;
using System.Reflection;
using UnityEngine;

namespace StatusFX.Elemental
{
	[DefaultStatusEffect(StatusEffectType.Electro, true)]
	internal sealed class ElectroDebuff : ElementalDebuff
	{
		// TODO: move to config
		private const float MAXDischargeTime = 3;
		private const float MINDischargeTime = 1;
		private const float DischargeRadius = 5;
		private const float DischargePoiseDamage = 10;
		private const float DischargeDamageMult = 0.3f;
		private const float DischargeAccumulatedDamageMult = 0.7f;
		private const float StatusSpreadMult = 0.5f;
		private const float StatusSpreadMAX = 0.7f;
		private float _accumulatedDamage;
		private float _nextDischargeTime;

		protected override void OnStart()
		{
			Target.OnTakeDamage += OnTakeDamage;
		}

		protected override void OnUpdate()
		{
			if (!IsStarted || Time.time < _nextDischargeTime)
				return;

			Discharge();
		}

		protected override void OnStop()
		{
			Target.OnTakeDamage -= OnTakeDamage;
			_accumulatedDamage = 0;
			_nextDischargeTime = 0;
		}

		private void OnTakeDamage(DamageInfo info, float factor)
		{
			if (!IsStarted)
				return;

			_accumulatedDamage += info.HealthAmount * factor;
		}

		private void Discharge()
		{
			_nextDischargeTime = GetNextDischargeTime();

			var dischargeDamage = GetDischargeTotalDamage();
			var dischargePoiseDamage = GetDischargePoiseDamage();
			Target.TakeDamage(new DamageInfo(DamageType.Elemental, dischargeDamage, dischargePoiseDamage));
			_accumulatedDamage -= dischargeDamage;

			var victims = StatusEffectsQueries.FindFriendsInSphere(Target, Target.GetTransform().position, DischargeRadius);
			if (victims.Count > 0)
			{
				var victimCount = victims.Count + 1; // Себя учитываем тоже
				var dischargeSingleDamage = dischargeDamage / victimCount;

				var appliedStatuses = Target.StatusFX.AsEnumerable()
					.OfType<IGaugeStatusEffect>()
					.Where(x => x.IsStarted)
					.Select(x => (statusType: x!.EffectType, statusStats:
						new StatusEffectInfo(
							Mathf.Min(StatusSpreadMAX, x.Amount * StatusSpreadMult * Strength),
							x.Damage / victimCount,
							x.Strength / victimCount))).ToList();

				foreach (var victim in victims)
				{
					victim.TakeDamage(new DamageInfo(DamageType.Elemental, dischargeSingleDamage,
						dischargePoiseDamage));

					foreach (var statusInfo in appliedStatuses)
						victim.ApplyStatus(statusInfo.statusType, statusInfo.statusStats);
				}
			}

			_accumulatedDamage = 0;
		}

		private float GetDischargePoiseDamage()
		{
			return DischargePoiseDamage * Strength;
		}

		private float GetDischargeTotalDamage()
		{
			var maxDamage = _accumulatedDamage + Damage;
			var calculatedDamage = _accumulatedDamage * DischargeAccumulatedDamageMult * Strength
			                       + Damage * DischargeDamageMult;
			return Mathf.Min(calculatedDamage, maxDamage);
		}

		private static float GetNextDischargeTime()
		{
			return Time.time + Random.Range(MINDischargeTime, MAXDischargeTime);
		}
	}
}