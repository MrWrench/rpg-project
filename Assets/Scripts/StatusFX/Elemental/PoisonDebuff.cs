using System.Reflection;
using UnityEngine;

namespace StatusFX
{
	[DefaultStatusEffect(EnumStatusType.POISON, true)]
	internal sealed class PoisonDebuff : GaugeStatusEffect
	{
		public override EnumStatusType type => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().type;

		public override bool isDebuff => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().isDebuff;

		public PoisonDebuff(Character target) : base(target) { }

		protected override void OnUpdate()
		{
			if (!isStarted)
				return;

			target.TakeDamage(new DamageInfo(EnumDamageType.ELEMENTAL, damage * baseDecayRate), Time.deltaTime);
		}
	}
}