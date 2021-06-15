using System;
using System.Collections.Generic;

namespace StatusFX
{
	public class StatusCollection : IStatusCollection
	{
		private readonly List<IStatusEffect> statusFX = new List<IStatusEffect>();

		private readonly Dictionary<EnumStatusType, IStatusEffect> statusFXDict =
			new Dictionary<EnumStatusType, IStatusEffect>();

		public event IStatusCollection.StatusEffectChangeDelegate? onStatusEffectStarted;
		public event IStatusCollection.StatusEffectChangeDelegate? onStatusEffectStopped;

		public IEnumerable<IStatusEffect> AsEnumerable() => statusFX;

		public IReadOnlyStatusEffect GetStatusEffect(EnumStatusType type)
		{
			return HasStatusEffectImplemented(type) ? statusFXDict[type] : StatusEffect.GetEmpty(type);
		}

		bool IStatusCollection.HasStatusEffectImplemented(EnumStatusType statusType) =>
			HasStatusEffectImplemented(statusType);

		private bool HasStatusEffectImplemented(EnumStatusType statusType) => statusFXDict.ContainsKey(statusType);

		void IStatusCollection.ImplementStatusEffect(IStatusEffect statusEffect)
		{
			if (statusEffect == null) throw new ArgumentNullException(nameof(statusEffect));
			if (HasStatusEffectImplemented(statusEffect.type)) throw new ArgumentException(nameof(statusEffect));

			statusFX.Add(statusEffect);
			statusFXDict.Add(statusEffect.type, statusEffect);
			statusEffect.onStarted += OnStatusEffectStarted;
			statusEffect.onStopped += OnStatusEffectStopped;
		}

		void IStatusCollection.UnimplementStatusEffect(IStatusEffect statusEffect)
		{
			if (statusEffect == null) throw new ArgumentNullException(nameof(statusEffect));
			if (!HasStatusEffectImplemented(statusEffect.type)) throw new ArgumentException(nameof(statusEffect));

			statusEffect.onStarted -= OnStatusEffectStarted;
			statusEffect.onStopped -= OnStatusEffectStopped;
			statusFX.Remove(statusEffect);
			statusFXDict.Remove(statusEffect.type);
		}

		private void OnStatusEffectStarted(IStatusEffect statusEffect) => onStatusEffectStarted?.Invoke(statusEffect);
		private void OnStatusEffectStopped(IStatusEffect statusEffect) => onStatusEffectStopped?.Invoke(statusEffect);
	}
}