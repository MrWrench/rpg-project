using System;
using System.Collections.Generic;

namespace StatusFX
{
	public interface IStatusCollection
	{
		delegate void StatusEffectChangeDelegate(IStatusEffect statusEffect);

		event StatusEffectChangeDelegate OnStatusEffectStarted;
		event StatusEffectChangeDelegate OnStatusEffectStopped;
		
		IEnumerable<IStatusEffect> AsEnumerable();
		IReadOnlyStatusEffect GetStatusEffectInfo(StatusEffectType effectType);
		IStatusEffect FindStatusEffect(StatusEffectType effectType);

		bool HasStatusEffectImplemented(StatusEffectType statusEffectType);
		void AddStatusEffect(IStatusEffect statusEffect);
		void RemoveStatusEffect(IStatusEffect statusEffect);
	}
}