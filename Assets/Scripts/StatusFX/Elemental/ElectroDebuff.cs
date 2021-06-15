using System.Linq;
using System.Reflection;
using UnityEngine;

namespace StatusFX.Elemental
{
	[DefaultStatusEffect(EnumStatusType.ELECTRO, true)]
	internal sealed class ElectroDebuff : ElementalDebuff
	{
		// TODO: move to config
		private const float MAX_DISCHARGE_TIME = 3;
		private const float MIN_DISCHARGE_TIME = 1;
		private const float DISCHARGE_RADIUS = 5;
		private const float DISCHARGE_POISE_DAMAGE = 10;
		private const float DISCHARGE_DAMAGE_MULT = 0.3f;
		private const float DISCHARGE_ACCUMULATED_DAMAGE_MULT = 0.7f;
		private const float STATUS_SPREAD_MULT = 0.5f;
		private const float STATUS_SPREAD_MAX = 0.7f;
		public override EnumStatusType type => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().type;
		public override bool isDebuff => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().isDebuff;

		private float accumulatedDamage;
		private float nextDischargeTime;

		protected override void OnStart()
		{
			target.onTakeDamage += OnTakeDamage;
		}

		protected override void OnUpdate()
		{
			if (!isStarted || Time.time < nextDischargeTime)
				return;

			Discharge();
		}

		protected override void OnStop()
		{
			target.onTakeDamage -= OnTakeDamage;
			accumulatedDamage = 0;
			nextDischargeTime = 0;
		}

		private void OnTakeDamage(DamageInfo info, float factor)
		{
			if (!isStarted)
				return;

			accumulatedDamage += info.healthAmount * factor;
		}

		private void Discharge()
		{
			nextDischargeTime = GetNextDischargeTime();

			var dischargeDamage = GetDischargeTotalDamage();
			var dischargePoiseDamage = GetDischargePoiseDamage();
			target.TakeDamage(new DamageInfo(EnumDamageType.ELEMENTAL, dischargeDamage, dischargePoiseDamage));
			accumulatedDamage -= dischargeDamage;

			var victims = StatusEffectsQueries.FindFriendsInSphere(target, target.transform.position, DISCHARGE_RADIUS);
			if (victims.Count > 0)
			{
				var victimCount = victims.Count + 1; // Себя учитываем тоже
				var dischargeSingleDamage = dischargeDamage / victimCount;

				var appliedStatuses = target.statusFX.AsEnumerable()
					.OfType<IGaugeStatusEffect?>()
					.Where(x => x != null && x.isStarted)
					.Select(x => (statusType: x!.type, statusStats:
						new StatusEffectInfo(
							Mathf.Min(STATUS_SPREAD_MAX, x.amount * STATUS_SPREAD_MULT * strength),
							x.damage / victimCount,
							x.strength / victimCount))).ToList();

				foreach (var victim in victims)
				{
					victim.TakeDamage(new DamageInfo(EnumDamageType.ELEMENTAL, dischargeSingleDamage,
						dischargePoiseDamage));

					foreach (var statusInfo in appliedStatuses)
						victim.ApplyStatus(statusInfo.statusType, statusInfo.statusStats);
				}
			}

			accumulatedDamage = 0;
		}

		private float GetDischargePoiseDamage()
		{
			return DISCHARGE_POISE_DAMAGE * strength;
		}

		private float GetDischargeTotalDamage()
		{
			var maxDamage = accumulatedDamage + damage;
			var calculatedDamage = accumulatedDamage * DISCHARGE_ACCUMULATED_DAMAGE_MULT * strength
			                       + damage * DISCHARGE_DAMAGE_MULT;
			return Mathf.Min(calculatedDamage, maxDamage);
		}

		private static float GetNextDischargeTime()
		{
			return Time.time + Random.Range(MIN_DISCHARGE_TIME, MAX_DISCHARGE_TIME);
		}
	}
}