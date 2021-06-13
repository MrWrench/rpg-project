namespace StatusFX
{
	public readonly struct EmptyGaugeStatusEffect : IReadOnlyGaugeStatusEffect
	{
		public bool isStarted => false;
		public EnumStatusType type { get; }
		public bool isDebuff { get; }
		public float amount => 0;
		public float damage => 0;
		public float strength => 0;

		public EmptyGaugeStatusEffect(EnumStatusType type, bool isDebuff)
		{
			this.type = type;
			this.isDebuff = isDebuff;
		}
	}
}