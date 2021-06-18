using UnityEngine;

namespace StatusFX.Elemental
{
	[CreateAssetMenu(fileName = "Hydro Debuff", menuName = "StatusFX/Hydro Debuff", order = 2)]
	internal sealed class HydroDebuff : ElementalDebuff
	{
		[SerializeField] private float _debuffAmount = 0.5f;

		public float DebuffAmount => _debuffAmount;
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