namespace StatusFX
{
	public readonly struct EmptyStatusEffect : IReadOnlyStatusEffect
	{
		public bool isStarted => false;
		public EnumStatusType type { get; }
		public bool isDebuff { get; }
		
		public EmptyStatusEffect(EnumStatusType type, bool isDebuff)
		{
			this.type = type;
			this.isDebuff = isDebuff;
		}
	}
}