using UnityEngine;

namespace StatusFX.Elemental.Configs
{
	[CreateAssetMenu(fileName = "Hydro Debuff Config", menuName = "Debuffs/Hydro", order = 0)]
	internal class HydroDebuffConfig : GaugeStatusEffectConfig<HydroDebuff>
	{
		[SerializeField] private float _debuffAmount = 0.5f;

		public float DebuffAmount => _debuffAmount;
	}
}