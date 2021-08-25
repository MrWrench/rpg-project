using StatusFX.Components;

namespace StatusFX.Elemental
{
	public sealed class FireStatus : ElementalEffect
	{
		private float DamagePercent => 0.3f;

		public FireStatus(Character target) : base(target)
		{
			Tag = StatusTag.Fire;
			
			AddComponent(new DOTComponent
			{
				DamagePercentage = DamagePercent, DamageType = DamageType.Elemental
			});

			AddComponent(new FireComponent());
		}
	}
}