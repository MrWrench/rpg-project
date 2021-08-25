namespace StatusFX.Components
{
	public class FireComponent : StatusComponent
	{
		public override void Tick()
		{
			var statusEffects = Owner.Target.StatusEffects;
			var count = statusEffects.Count;
			for (int i = 0; i < count; i++)
			{
				var effect = statusEffects.Get(i);
				if (effect == Owner || effect.CurrentStacks <= 0)
					continue;

				Explode();
				return;
			}
		}

		private void Explode()
		{
			var totalDamage = 0f;
			var statusEffects = Owner.Target.StatusEffects;
			var count = statusEffects.Count;

			for (int i = 0; i < count; i++)
			{
				var effect = statusEffects.Get(i);
				if (effect.CurrentStacks > 0)
				{
					var expirationComponent = effect.GetComponent<ExpirationComponent>();
					var durationLeftPercent = expirationComponent.TimeLeft / expirationComponent.Duration;
					totalDamage += effect.Damage * durationLeftPercent * Owner.Strength;
				}
			}

			var dinfo = new DamageInfo {HealthAmount = totalDamage, Inflictor = this, Type = DamageType.Elemental};
			Owner.Target.ApplyDamage(dinfo);
			
			for (int i = 0; i < count; i++)
			{
				var effect = statusEffects.Get(i);
				effect.ChangeStacks(-effect.CurrentStacks);
			}
		}
	}
}