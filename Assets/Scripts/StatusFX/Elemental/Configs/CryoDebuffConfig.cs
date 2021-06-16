using UnityEngine;

namespace StatusFX.Elemental.Configs
{
	[CreateAssetMenu(fileName = "Cryo Debuff Config", menuName = "Debuffs/Cryo", order = 0)]
	internal class CryoDebuffConfig : GaugeStatusEffectConfig<CryoDebuff>
	{
		[SerializeField] private float _poiseDebuff = 10;

		public float PoiseDebuff => _poiseDebuff;
	}
}