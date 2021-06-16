using System;
using UnityEngine;

namespace StatusFX
{
	public abstract class GaugeStatusEffectConfig<T> : StatusEffectConfig, IGaugeStatusEffectConfig
		where T : IGaugeStatusEffect
	{
		public override Type StatusEffectClassType => typeof(T);
		public float GetBaseDecayRate => _getBaseDecayRate;

		[SerializeField] private float _getBaseDecayRate;
	}
}