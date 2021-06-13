using System.Linq;
using System.Reflection;
using UnityEngine;

namespace StatusFX
{
	[DefaultStatusEffect(EnumStatusType.FIRE, true)]
	internal sealed class FireDebuff : GaugeStatusEffect
	{
		private const float EXPLOSION_RADIUS = 5;
		private const float STATUS_SPREAD_MULT = 0.4f;
		private const float EXPLOSION_DAMAGE_MULT = 0.4f;
		private const float EXPLOSION_STRENGTH_MULT = 0.4f;
		private const float EXPLOSION_POISE_DAMAGE = 40;
		private const float HYDRO_STRENGTH_MULT = 0.5f;

		public override EnumStatusType type => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().type;

		public override bool isDebuff => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().isDebuff;

		public FireDebuff(Character target) : base(target) { }

		protected override void OnStart()
		{
			if (!TryExplode())
				target.onStatusEffectStarted += OnStatusEffectStarted;
		}

		protected override void OnUpdate()
		{
			if (!isStarted)
				return;

			target.TakeDamage(new DamageInfo(EnumDamageType.ELEMENTAL, damage * baseDecayRate), Time.deltaTime);
		}

		protected override void OnStop()
		{
			target.onStatusEffectStarted -= OnStatusEffectStarted;
		}

		private void OnStatusEffectStarted(IStatusEffect statusEffect)
		{
			TryExplode();
		}

		private bool TryExplode()
		{
			if (!isStarted)
				return false;

			var gauges = target.GetGaugeStatusFX();
			var count = gauges.Count;

			var electroStrength = 0f;
			var cryoStrength = 0f;
			var hydroStrength = 0f;
			var totalAmount = amount;
			var totalStrength = strength;
			var statusCount = 1;

			var totalDamage = 0f;
			for (int i = 0; i < count; i++)
			{
				var status = gauges[i];
				if (status.isStarted && status.type != type)
				{
					totalDamage += status.damage * status.amount * strength;
					totalStrength += status.strength;
					totalAmount += status.amount;
					statusCount++;

					switch (status.type)
					{
						case EnumStatusType.ELECTRO:
							electroStrength = status.strength;
							break;
						case EnumStatusType.HYDRO:
							hydroStrength = status.strength;
							break;
						case EnumStatusType.CRYO:
							cryoStrength = status.strength;
							break;
					}
				}
			}

			var poiseDamage = EXPLOSION_POISE_DAMAGE * cryoStrength;

			if (totalDamage > 0)
			{
				totalDamage += damage * amount * strength; // Прибавляем сам огонь
				totalDamage += totalDamage * hydroStrength * HYDRO_STRENGTH_MULT; // Прибавляем бонус от воды

				for (int i = 0; i < count; i++)
					gauges[i].Clear();

				target.TakeDamage(new DamageInfo(EnumDamageType.ELEMENTAL, totalDamage, poiseDamage));

				if (electroStrength > 0)
				{
					ExplodeAOE(totalDamage, totalAmount, totalStrength, statusCount, poiseDamage);
				}

				return true;
			}

			return false;
		}

		private void ExplodeAOE(float totalDamage, float totalAmount, float totalStrength, int statusCount,
			float poiseDamage)
		{
			var explosionDamage = totalDamage * EXPLOSION_DAMAGE_MULT;
			var statusAmount = Mathf.Min(totalAmount * STATUS_SPREAD_MULT, 1);
			var explosionStength = totalStrength / statusCount * EXPLOSION_STRENGTH_MULT;

			var victims = StatusEffectsQueries.FindFriendsInSphere(target, target.transform.position, EXPLOSION_RADIUS);

			if (victims.Count > 0)
			{
				foreach (var victim in victims)
				{
					victim.TakeDamage(new DamageInfo(EnumDamageType.ELEMENTAL, explosionDamage, poiseDamage));
					victim.ApplyStatus(EnumStatusType.FIRE,
						new StatusEffectInfo(statusAmount, explosionDamage, explosionStength));
				}
			}
		}
	}
}