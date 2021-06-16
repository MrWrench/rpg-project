using Reflection;

namespace StatusFX
{
	internal readonly struct EmptyStatusEffect : IReadOnlyStatusEffect
	{
		public bool IsStarted => false;
		public StatusEffectType EffectType { get; }
		public bool IsDebuff { get; }
		
		public EmptyStatusEffect(StatusEffectType effectType)
		{
			this.EffectType = effectType;
			IsDebuff = DefaultStatusEffectPool.FindDefaultType(effectType)?.GetCustomAttribute<DefaultStatusEffectAttribute>()
				.IsDebuff ?? false;
		}
	}
}