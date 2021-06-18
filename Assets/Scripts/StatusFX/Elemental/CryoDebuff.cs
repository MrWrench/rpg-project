using UnityEngine;

namespace StatusFX.Elemental
{
	[CreateAssetMenu(fileName = "Cryo Debuff", menuName = "StatusFX/Cryo Debuff", order = 2)]
	internal sealed class CryoDebuff : ElementalDebuff
	{
		public float PoiseDebuff => _poiseDebuff;
		
		[SerializeField] private float _poiseDebuff = 10;
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