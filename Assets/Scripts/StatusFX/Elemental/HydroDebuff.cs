using System.Reflection;

namespace StatusFX.Elemental
{
	[DefaultStatusEffect(StatusEffectType.Hydro, true)]
	internal sealed class HydroDebuff : ElementalDebuff
	{
		private const float DebuffAmount = 0.5f;
		private float _appliedAmount;

		protected override void OnStart()
		{
			_appliedAmount = DebuffAmount * Strength;
			Target.DebuffDurationMult += _appliedAmount;
		}

		protected override void OnStop()
		{
			Target.DebuffDurationMult -= _appliedAmount;
			_appliedAmount = 0;
		}
	}
}