using System;
using System.Collections.Generic;

namespace StatusFX
{
	public class StatusCollection : IStatusCollection
	{
		private readonly List<IStatusEffect> _statusFX = new List<IStatusEffect>();

		private readonly Dictionary<StatusEffectType, IStatusEffect> _statusFXDict =
			new Dictionary<StatusEffectType, IStatusEffect>();

		public event IStatusCollection.StatusEffectChangeDelegate OnStatusEffectStarted;
		public event IStatusCollection.StatusEffectChangeDelegate OnStatusEffectStopped;

		public IEnumerable<IStatusEffect> AsEnumerable() => _statusFX;

		public IReadOnlyStatusEffect GetStatusEffect(StatusEffectType effectType)
		{
			return HasStatusEffectImplemented(effectType) ? _statusFXDict[effectType] : StatusEffect.GetEmpty(effectType);
		}

		bool IStatusCollection.HasStatusEffectImplemented(StatusEffectType statusEffectType) =>
			HasStatusEffectImplemented(statusEffectType);

		private bool HasStatusEffectImplemented(StatusEffectType statusEffectType) => _statusFXDict.ContainsKey(statusEffectType);

		void IStatusCollection.ImplementStatusEffect(IStatusEffect statusEffect)
		{
			if (statusEffect == null) throw new ArgumentNullException(nameof(statusEffect));
			if (HasStatusEffectImplemented(statusEffect.EffectType)) throw new ArgumentException(nameof(statusEffect));

			_statusFX.Add(statusEffect);
			_statusFXDict.Add(statusEffect.EffectType, statusEffect);
			statusEffect.OnStarted += NotifyStatusEffectStarted;
			statusEffect.OnStopped += NotifyStatusEffectStopped;
		}

		void IStatusCollection.UnimplementStatusEffect(IStatusEffect statusEffect)
		{
			if (statusEffect == null) throw new ArgumentNullException(nameof(statusEffect));
			if (!HasStatusEffectImplemented(statusEffect.EffectType)) throw new ArgumentException(nameof(statusEffect));

			statusEffect.OnStarted -= NotifyStatusEffectStarted;
			statusEffect.OnStopped -= NotifyStatusEffectStopped;
			_statusFX.Remove(statusEffect);
			_statusFXDict.Remove(statusEffect.EffectType);
		}

		private void NotifyStatusEffectStarted(IStatusEffect statusEffect) => OnStatusEffectStarted?.Invoke(statusEffect);
		private void NotifyStatusEffectStopped(IStatusEffect statusEffect) => OnStatusEffectStopped?.Invoke(statusEffect);
	}
}