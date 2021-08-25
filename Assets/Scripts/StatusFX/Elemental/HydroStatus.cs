using StatusFX.Components;

namespace StatusFX.Elemental
{
	public sealed class HydroStatus : ElementalEffect
	{
		private float DebuffPerStack => 0.3f;

		public HydroStatus(Character target) : base(target)
		{
			Tag = StatusTag.Hydro;
			AddComponent(new HydroComponent() {DebuffPerStack = DebuffPerStack});
		}
	}
}