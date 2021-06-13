using System.Reflection;
using UnityEngine;

namespace StatusFX.Elemental
{
	[DefaultStatusEffect(EnumStatusType.POISON, true)]
	internal sealed class PoisonDebuff : ElementalDebuff
	{
		public override EnumStatusType type => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().type;

		public override bool isDebuff => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().isDebuff;

		protected override void OnUpdate()
		{
			if (!isStarted)
				return;

			target.TakeDamage(new DamageInfo(EnumDamageType.ELEMENTAL, damage * baseDecayRate), Time.deltaTime);
		}
	}
}