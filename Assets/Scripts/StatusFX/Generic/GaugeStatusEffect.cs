using System;
using UnityEngine;

namespace StatusFX.Generic
{
	public abstract class GaugeStatusEffect<T> : StatusEffect<T>, IGaugeStatusEffect
		where T : IStatusFXCarrier, IStatsCarrier, ISceneObject
	{
		public float amount { get; private set; }
		public float strength { get; private set; }
		public float damage { get; private set; }
		public float baseDecayRate => 0.1f; // TODO: To config

		protected float decayRate => baseDecayRate / (1 + target.debuffDurationMult);

		public void Add(StatusEffectInfo effectInfo, float factor = 1)
		{
			if (effectInfo.amount > 1 || effectInfo.amount < 0)
				throw new ArgumentOutOfRangeException($"{nameof(effectInfo)}.{nameof(effectInfo.amount)}");

			if (effectInfo.damage < 0)
				throw new ArgumentOutOfRangeException($"{nameof(effectInfo)}.{nameof(effectInfo.damage)}");

			if (effectInfo.strength < 0)
				throw new ArgumentOutOfRangeException($"{nameof(effectInfo)}.{nameof(effectInfo.strength)}");

			if (factor <= 0)
				throw new ArgumentOutOfRangeException(nameof(factor));

			if (isStarted)
				return;

			var addedAmount = Mathf.Min(1 - amount, effectInfo.amount * factor);
			if (addedAmount > 0)
			{
				strength = (strength * amount + effectInfo.strength * addedAmount) / (amount + addedAmount);
				damage = (damage * amount + effectInfo.damage * addedAmount) / (amount + addedAmount);
				amount += addedAmount;
			}

			if (amount >= 1)
				Start();
		}

		public void Clear()
		{
			Stop();
		}

		protected sealed override void Update()
		{
			if (amount > 0)
			{
				amount -= decayRate * Time.deltaTime;

				if (amount <= 0)
				{
					Stop();
				}
			}

			base.Update();
		}

		public sealed override void Start()
		{
			amount = 1;
			base.Start();
		}

		public sealed override void Stop()
		{
			amount = 0;
			base.Stop();
		}

		public static IReadOnlyGaugeStatusEffect GetEmpty(EnumStatusType requiredType)
		{
			return new EmptyGaugeStatusEffect(requiredType);
		}
	}
}