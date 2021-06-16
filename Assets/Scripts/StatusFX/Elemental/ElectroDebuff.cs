using System.Linq;
using StatusFX.Elemental.Configs;
using UnityEngine;

namespace StatusFX.Elemental
{
	internal sealed class ElectroDebuff : ElementalDebuff<ElectroDebuffConfig>
	{
		// TODO: move to config
		private float MaxDischargeTime => Config.MaxDischargeTime;
		private float MinDischargeTime => Config.MinDischargeTime;
		private float DischargeRadius => Config.DischargeRadius;
		private float DischargePoiseDamage => Config.DischargePoiseDamage;
		private float DischargeDamageMult => Config.DischargeDamageMult;
		private float DischargeAccumulatedDamageMult => Config.DischargeAccumulatedDamageMult;
		private float StatusSpreadMult => Config.StatusSpreadMult;
		private float StatusSpreadMax => Config.StatusSpreadMax;
		
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
							Mathf.Min(StatusSpreadMax, x.Amount * StatusSpreadMult * Strength),
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

		private float GetNextDischargeTime()
		{
			return Time.time + Random.Range(MinDischargeTime, MaxDischargeTime);
		}
	}
}