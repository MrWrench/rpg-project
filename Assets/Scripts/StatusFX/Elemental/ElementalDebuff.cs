using Reflection;
using StatusFX.Generic;

namespace StatusFX.Elemental
{
	public abstract class ElementalDebuff : GaugeStatusEffect<ICombatUnit>
	{
		public override StatusEffectType EffectType => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().EffectType;
		public override bool IsDebuff => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().IsDebuff;
	}
}