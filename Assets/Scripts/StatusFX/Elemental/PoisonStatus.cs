using StatusFX.Components;

namespace StatusFX.Elemental
{
	public sealed class PoisonStatus : ElementalEffect
	{
		private float DamagePercent => 0.3f;

		public PoisonStatus()
		{
			Tag = StatusTag.Poison;
			
			AddComponent(new DOTComponent
			{
				DamagePercentage = DamagePercent, DamageType = DamageType.Elemental
			});
		}
	}
}