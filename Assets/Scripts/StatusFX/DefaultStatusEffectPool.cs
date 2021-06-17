using System;
using System.Collections.Generic;
using System.Linq;
using StatusFX.Elemental;

namespace StatusFX
{
	internal static class DefaultStatusEffectPool
	{
		private static readonly Dictionary<StatusEffectType, IStatusEffect> DefaultStatusFX;

		static DefaultStatusEffectPool()
		{
			DefaultStatusFX =
				StatusFXDefaults.instance.DefaultEffects.ToDictionary(x => x.EffectType, x => (IStatusEffect) x);
		}

		internal static IStatusEffect Instantiate(StatusEffectType statusEffectType)
		{
			if (!DefaultStatusFX.ContainsKey(statusEffectType))
				throw new ArgumentException($"Default status effect for type {statusEffectType} does not exist");

			var statusEffect = DefaultStatusFX[statusEffectType];
			return statusEffect.CreateInstance();
		}

		public static IReadOnlyStatusEffect FindDefaultConfig(StatusEffectType statusEffectType)
		{
			return DefaultStatusFX.TryGetValue(statusEffectType, out var result) ? result : null;
		}
	}
}