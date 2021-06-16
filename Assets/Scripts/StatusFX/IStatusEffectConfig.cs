using System;

namespace StatusFX
{
	// Параллельная иерархия со IStatusEffect
	// Мб сам StatusEffect сделать ScriptableObject?
	public interface IStatusEffectConfig
	{
		public bool IsDebuff { get; }
		public StatusEffectType EffectType { get; }
		public Type StatusEffectClassType { get; }
	}
}