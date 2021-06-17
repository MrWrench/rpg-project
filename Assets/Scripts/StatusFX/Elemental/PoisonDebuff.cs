using UnityEngine;

namespace StatusFX.Elemental
{
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