using StatusFX.Elemental.Configs;

namespace StatusFX.Elemental
{
	internal sealed class HydroDebuff : ElementalDebuff<HydroDebuffConfig>
	{
		private float DebuffAmount => Config.DebuffAmount;
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