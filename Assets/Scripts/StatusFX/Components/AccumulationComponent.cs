using System;
using UnityEngine;

namespace StatusFX.Components
{
	public class AccumulationComponent : StatusComponent
	{
		public float BaseDecayRate { get; set; }
		public float Accumulation { get; private set; }
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
			if (Owner.CurrentStacks == 0)
			{
				Owner.Damage = 0;
				Owner.Strength = 0;
			}
		}

		public void AddAccumulation(float amount, float damage, float strength)
		{
			if(Owner.CurrentStacks == Owner.MaxStacks)
				return;
			
			float addedAmount;
			if (Owner.CurrentStacks + 1 < Owner.MaxStacks)
				addedAmount = amount;
			else
				addedAmount = Mathf.Min(1 - Accumulation, amount);

			if (addedAmount > 0)
			{
				Owner.Strength = (Owner.Strength * Accumulation + strength * addedAmount) / (Accumulation + addedAmount);
				Owner.Damage = (Owner.Damage * Accumulation + damage * addedAmount) / (Accumulation + addedAmount);
				Accumulation += addedAmount;
			}
			
			if(Accumulation >= 1)
			{
				Owner.ChangeStacks(1);
				Accumulation -= 1;
			}
		}
	}
}