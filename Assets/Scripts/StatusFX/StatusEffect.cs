namespace StatusFX
{
	public class StatusEffect : Wrenge.StatusFX.StatusEffect
	{
		public StatusTag StatusTag { get; set; }
		public Character Target { get; set; }

		public StatusEffect()
		{
			Time = new UnityTimeProvider();
		}
	}
}