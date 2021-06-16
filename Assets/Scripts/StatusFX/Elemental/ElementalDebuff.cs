using Reflection;

namespace StatusFX.Elemental
{
	public abstract class ElementalDebuff<TConfig> : GaugeStatusEffect<ICombatUnit, TConfig>
		where TConfig : IGaugeStatusEffectConfig
	{
		public override StatusEffectType EffectType => Config.EffectType;
		public override bool IsDebuff => Config.IsDebuff;
	}
}