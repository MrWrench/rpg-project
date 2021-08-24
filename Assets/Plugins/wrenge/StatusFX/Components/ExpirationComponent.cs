namespace Wrenge.StatusFX.Components
{
	public class ExpirationComponent : StatusComponent
	{
		public float ExpirationTime;
		public float ExpirationStamp;

		protected override void OnAdded()
		{
			if (Owner.CurrentStacks > 0)
				ExpirationStamp = Time.CurrentTime + ExpirationTime;
		}

		public override void Tick()
		{
			if(Owner.CurrentStacks <= 0)
				return;
			
			if(Time.CurrentTime > ExpirationStamp)
				Owner.ChangeStacks(-1);
		}

		protected override void OnStacksChanged(int deltaStacks)
		{
			ExpirationStamp = Time.CurrentTime + ExpirationTime;
		}
	}
}