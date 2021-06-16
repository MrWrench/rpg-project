using UnityEngine;

namespace StatusFX.Elemental.Configs
{
	[CreateAssetMenu(fileName = "Electro Debuff Config", menuName = "Debuffs/Electro", order = 0)]
	internal class ElectroDebuffConfig : GaugeStatusEffectConfig<ElectroDebuff>
	{
		[SerializeField] private float _maxDischargeTime = 3;
		[SerializeField] private float _minDischargeTime = 1;
		[SerializeField] private float _dischargeRadius = 5;
		[SerializeField] private float _dischargePoiseDamage = 10;
		[SerializeField] private float _dischargeDamageMult = 0.3f;
		[SerializeField] private float _dischargeAccumulatedDamageMult = 0.7f;
		[SerializeField] private float _statusSpreadMult = 0.5f;
		[SerializeField] private float _statusSpreadMax = 0.7f;

		public float MaxDischargeTime => _maxDischargeTime;
		public float MinDischargeTime => _minDischargeTime;
		public float DischargeRadius => _dischargeRadius;
		public float DischargePoiseDamage => _dischargePoiseDamage;
		public float DischargeDamageMult => _dischargeDamageMult;
		public float DischargeAccumulatedDamageMult => _dischargeAccumulatedDamageMult;
		public float StatusSpreadMult => _statusSpreadMult;
		public float StatusSpreadMax => _statusSpreadMax;
	}
}