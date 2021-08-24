namespace StatusFX.Components
{
	public class HydroComponent : StatusComponent
	{
		public float DebuffPerStack { get; set; }
		private float _debuffDelta;
		
		protected override void OnAdded()
		{
			NotifyStacksChanged(Owner.CurrentStacks);
		}

		protected override void OnStacksChanged(int deltaStacks)
		{
			var debuffAmount = deltaStacks * DebuffPerStack;
			Owner.Target.Stats.StatusDuration += debuffAmount;
			_debuffDelta += debuffAmount;
		}

		protected override void OnRemoved(Wrenge.StatusFX.StatusEffect previousOwner)
		{
			Owner.Target.Stats.StatusDuration -= _debuffDelta;
		}
	}
}