using StatusFX.Components;

namespace StatusFX.Elemental
{
	public class ElectroStatus : ElementalEffect
	{
		private static int DischargePeriod => 1;
		private static int DischargeRadius => 1;
		private static float DischargeDamageMult => 0.3f;
		
		public ElectroStatus()
		{
			Tag = StatusTag.Electro;
			AddComponent(new ElectroComponent
			{
				DischargeDamageMult = DischargeDamageMult,
				DischargeRadius = DischargeRadius,
				DischargePeriod = DischargePeriod
			});
		}
	}
}