using System;

namespace StatusFX.Components
{
	public class AccumulationComponent : StatusComponent
	{
		public float BaseDecayRate { get; set; }
		public float Accumulation { get; set; }
		public float DecayRate => BaseDecayRate * Owner.Target.Stats.StatusDuration;

		public override void Tick()
		{
			if(Accumulation > 1)
				Owner.ChangeStacks(1);

			if (Accumulation > 0)
				Accumulation = Math.Max(Accumulation - DecayRate * Time.DeltaTime, 0);	
		}

		protected override void OnStacksChanged(int deltaStacks)
		{
			Accumulation = 0;
		}
		 
	}
}