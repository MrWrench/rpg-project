using StatusFX.Components;

namespace StatusFX.Elemental
{
	public sealed class CryoStatus : ElementalEffect
	{
		private int DebuffPerStack => 10;
		
		public CryoStatus(Character target) : base(target)
		{
			Tag = StatusTag.Cryo;
			AddComponent(new CryoComponent {DebuffPerStack = DebuffPerStack});
		}

	}
}