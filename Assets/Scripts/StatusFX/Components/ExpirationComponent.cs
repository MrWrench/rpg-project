using System;

namespace StatusFX.Components
{
	public class ExpirationComponent : StatusComponent
	{
		public float Duration { get; set; }
		public float TimeLeft { get; set; }
		public float DecayRate => Time.DeltaTime / Owner.Target.Stats.StatusDuration;

		public override void Tick()
		{
			if(Owner.CurrentStacks <= 0)
				return;

			TimeLeft -= DecayRate;
			if(TimeLeft <= 0)
				Owner.ChangeStacks(-1);
		}

		protected override void OnStacksChanged(int deltaStacks)
		{
			TimeLeft = Duration;
		}
		 
	}
}