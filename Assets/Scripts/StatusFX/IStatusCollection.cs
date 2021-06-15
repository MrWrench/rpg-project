using System.Collections.Generic;

namespace StatusFX
{
	public interface IStatusCollection
	{
		delegate void StatusEffectChangeDelegate(IStatusEffect statusEffect);

		event StatusEffectChangeDelegate onStatusEffectStarted;
		event StatusEffectChangeDelegate onStatusEffectStopped;
		
		IEnumerable<IStatusEffect> AsEnumerable();
		IReadOnlyStatusEffect GetStatusEffect(EnumStatusType type);

		// Кажется я что-то делаю не так
		internal bool HasStatusEffectImplemented(EnumStatusType statusType);
		internal void ImplementStatusEffect(IStatusEffect statusEffect);
		internal void UnimplementStatusEffect(IStatusEffect statusEffect);
	}
}