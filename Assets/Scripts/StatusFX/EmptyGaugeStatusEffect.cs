using System.Reflection;

namespace StatusFX
{
	internal readonly struct EmptyGaugeStatusEffect : IReadOnlyGaugeStatusEffect
	{
		public bool IsStarted => false;
		public StatusEffectType EffectType { get; }
		public bool IsDebuff { get; }
		public float Amount => 0;
		public float Damage => 0;
		public float Strength => 0;

		public EmptyGaugeStatusEffect(StatusEffectType effectType)
		{
			EffectType = effectType;
			IsDebuff = DefaultStatusEffectPool.FindDefaultConfig(effectType)?.IsDebuff ?? false;
		}
	}
}