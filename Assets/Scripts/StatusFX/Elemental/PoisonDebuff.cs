using UnityEngine;

namespace StatusFX.Elemental
{
	[CreateAssetMenu(fileName = "Poison Debuff", menuName = "StatusFX/Poison Debuff", order = 2)]
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