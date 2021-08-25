using StatusFX.Components;

namespace StatusFX.Elemental
{
	public sealed class HydroStatus : ElementalEffect
	{
		private float DebuffPerStack => 0.3f;

		public HydroStatus()
		{
			Tag = StatusTag.Hydro;
			AddComponent(new HydroComponent() {DebuffPerStack = DebuffPerStack});
		}
	}
}