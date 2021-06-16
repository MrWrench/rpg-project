using StatusFX.Elemental.Configs;

namespace StatusFX.Elemental
{
	internal sealed class CryoDebuff : ElementalDebuff<CryoDebuffConfig>
	{
		private float PoiseDebuff => Config.PoiseDebuff;
		private float _appliedAmount;

		protected override void OnStart()
		{
			_appliedAmount = PoiseDebuff * Strength;
			Target.PoiseDamageDebuff += _appliedAmount;
		}

		protected override void OnStop()
		{
			Target.PoiseDamageDebuff -= _appliedAmount;
			_appliedAmount = 0;
		}
	}
}