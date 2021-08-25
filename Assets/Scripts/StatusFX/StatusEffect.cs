namespace StatusFX
{
	public class StatusEffect : Wrenge.StatusFX.StatusEffect
	{
		public StatusTag Tag { get; set; }
		public Character Target { get; private set; }

		public float Damage { get; set; }
		public float Strength { get; set; }

		public StatusEffect(Character target)
		{
			Target = target;
			Time = new UnityTimeProvider();
		}
	}
}