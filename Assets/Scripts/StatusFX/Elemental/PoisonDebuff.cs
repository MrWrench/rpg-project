using System.Reflection;
using UnityEngine;

namespace StatusFX.Elemental
{
	[DefaultStatusEffect(StatusEffectType.Poison, true)]
	internal sealed class PoisonDebuff : ElementalDebuff
	{
		protected override void OnUpdate()
		{
			if (!IsStarted)
				return;

			Target.TakeDamage(new DamageInfo(DamageType.Elemental, Damage * BaseDecayRate), Time.deltaTime);
		}
	}
}