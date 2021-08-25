namespace StatusFX.Components
{
	public class DOTComponent : StatusComponent
	{
		public float DamageAmount { get; set; }

		public override void Tick()
		{
			Owner.Target.ApplyDamage(new DamageInfo {HealthAmount = DamageAmount});
		}
	}
}