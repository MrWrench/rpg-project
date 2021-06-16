using System.Collections.Generic;

namespace StatusFX
{
	public interface IStatusCollection
	{
		delegate void StatusEffectChangeDelegate(IStatusEffect statusEffect);

		event StatusEffectChangeDelegate OnStatusEffectStarted;
		event StatusEffectChangeDelegate OnStatusEffectStopped;
		
		IEnumerable<IStatusEffect> AsEnumerable();
		IReadOnlyStatusEffect GetStatusEffect(StatusEffectType effectType);

		// Кажется я что-то делаю не так
		internal bool HasStatusEffectImplemented(StatusEffectType statusEffectType);
		internal void ImplementStatusEffect(IStatusEffect statusEffect);
		internal void UnimplementStatusEffect(IStatusEffect statusEffect);
	}
}