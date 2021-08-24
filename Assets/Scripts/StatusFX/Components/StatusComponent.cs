namespace StatusFX
{
	public abstract class StatusComponent : Wrenge.StatusFX.StatusComponent
	{
		public new StatusEffect Owner => base.Owner as StatusEffect;
	}
}