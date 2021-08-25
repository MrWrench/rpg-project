using StatusFX.Components;

namespace StatusFX.Elemental
{
	public sealed class CryoStatus : ElementalEffect
	{
		private int DebuffPerStack => 10;
		
		public CryoStatus()
		{
			Tag = StatusTag.Cryo;
			AddComponent(new CryoComponent {DebuffPerStack = DebuffPerStack});
		}

	}
}