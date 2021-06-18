using System;
using UnityEngine;

namespace StatusFX
{
	public abstract class GaugeStatusEffect<TTarget> : StatusEffect<TTarget>, IGaugeStatusEffect
		where TTarget : IStatusFXCarrier, IStatsCarrier, ISceneObject
	{
		public float Amount { get; private set; }
		public float Strength { get; private set; }
		public float Damage { get; private set; }

		public float BaseDecayRate => _baseDecayRate;
		[SerializeField] private float _baseDecayRate = 0.1f;

		public void Add(StatusEffectInfo effectInfo, float factor = 1)
		{
			if (effectInfo.Amount > 1 || effectInfo.Amount < 0)
				throw new ArgumentOutOfRangeException($"{nameof(effectInfo)}.{nameof(effectInfo.Amount)}");

			if (effectInfo.Damage < 0)
				throw new ArgumentOutOfRangeException($"{nameof(effectInfo)}.{nameof(effectInfo.Damage)}");

			if (effectInfo.Strength < 0)
				throw new ArgumentOutOfRangeException($"{nameof(effectInfo)}.{nameof(effectInfo.Strength)}");

			if (factor <= 0)
				throw new ArgumentOutOfRangeException(nameof(factor));

			if (IsStarted)
				return;

			var addedAmount = Mathf.Min(1 - Amount, effectInfo.Amount * factor);
			if (addedAmount > 0)
			{
				Strength = (Strength * Amount + effectInfo.Strength * addedAmount) / (Amount + addedAmount);
				Damage = (Damage * Amount + effectInfo.Damage * addedAmount) / (Amount + addedAmount);
				Amount += addedAmount;
			}

			if (Amount >= 1)
				Start();
		}

		public void Clear()
		{
			Stop();
		}

		protected sealed override void Update()
		{
			if (Amount > 0)
			{
				Amount -= GetFinalDecayRate() * Time.deltaTime;

				if (Amount <= 0)
				{
					Stop();
				}
			}

			base.Update();
		}

		public sealed override void Start()
		{
			Amount = 1;
			base.Start();
		}

		public sealed override void Stop()
		{
			Amount = 0;
			base.Stop();
		}

		public static IReadOnlyGaugeStatusEffect GetEmpty(StatusEffectType requiredEffectType)
		{
			return new EmptyGaugeStatusEffect(requiredEffectType);
		}
		
		protected float GetFinalDecayRate()
		{
			return BaseDecayRate / (1 + Target.DebuffDurationMult);
		}
	}
}