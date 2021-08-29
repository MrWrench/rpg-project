using Game;

namespace StatusFX.Components
{
	public class DOTComponent : StatusComponent
	{
		public float DamagePercentage { get; set; }
		public DamageType DamageType { get; set; } = DamageType.Elemental;

		public override void Tick()
		{
			var damageAmount = DamagePercentage * Owner.CurrentStacks * Owner.Damage;
			Owner.Target.ApplyDamage(new DamageInfo {HealthAmount = damageAmount, Inflictor = Owner, Type = DamageType});
		}
	}
}