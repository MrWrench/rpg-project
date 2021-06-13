using Reflection;

namespace StatusFX
{
	internal readonly struct EmptyStatusEffect : IReadOnlyStatusEffect
	{
		public bool isStarted => false;
		public EnumStatusType type { get; }
		public bool isDebuff { get; }
		
		public EmptyStatusEffect(EnumStatusType type)
		{
			this.type = type;
			isDebuff = DefaultStatusEffectPool.FindDefaultType(type)?.GetCustomAttribute<DefaultStatusEffectAttribute>()
				.isDebuff ?? false;
		}
	}
}