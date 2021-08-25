namespace StatusFX.Components
{
	public class CryoComponent : StatusComponent
	{
		public float DebuffPerStack { get; set; }
		private float _debuffDelta;
		
		protected override void OnAdded()
		{
			if(Owner.CurrentStacks > 0)
				NotifyStacksChanged(Owner.CurrentStacks);
		}

		protected override void OnStacksChanged(int deltaStacks)
		{
			var debuffAmount = deltaStacks * DebuffPerStack * Owner.Strength;
			Owner.Target.Stats.PoiseDamageDebuff += debuffAmount;
			_debuffDelta += debuffAmount;
		}

		protected override void OnRemoved(Wrenge.StatusFX.StatusEffect previousOwner)
		{
			Owner.Target.Stats.PoiseDamageDebuff -= _debuffDelta;
		}
	}
}