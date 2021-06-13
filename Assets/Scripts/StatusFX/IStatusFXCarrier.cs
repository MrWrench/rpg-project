using System.Collections.Generic;

namespace StatusFX
{
	public interface IStatusFXCarrier : IUpdateProvider
	{
		delegate void StatusEffectChangeDelegate(IStatusEffect statusEffect);
		event StatusEffectChangeDelegate onStatusEffectStarted;
		event StatusEffectChangeDelegate onStatusEffectStoped;

		void ApplyStatus(EnumStatusType type, StatusEffectInfo info, float factor = 1);
		void ClearStatus(EnumStatusType type);
		IEnumerable<IStatusEffect> GetStatusFX();
		IReadOnlyStatusEffect GetStatusEffect(EnumStatusType type);

		// Кажется я что-то делаю не так
		internal bool HasStatusEffectImplemented(EnumStatusType statusType);
		internal void ImplementStatusEffect(IStatusEffect statusEffect);
		internal void UnimplementStatusEffect(IStatusEffect statusEffect);
	}
}