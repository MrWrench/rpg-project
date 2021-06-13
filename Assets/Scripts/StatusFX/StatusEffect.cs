namespace StatusFX
{
	public abstract class StatusEffect : IStatusEffect
	{
		public abstract EnumStatusType type { get; }
		public bool isStarted { get; private set; }
		public abstract bool isDebuff { get; }
		public abstract event IStatusEffect.StartDelegate? onStarted;
		public abstract event IStatusEffect.StopDelegate? onStoped;

		public abstract void Start();

		public abstract void Stop();

		public abstract void LinkNewTarget(IStatusFXCarrier newTarget);

		public abstract void UnlinkCurrentTarget();

		public static IStatusEffect GetDefault(EnumStatusType requiredType)
		{
			return DefaultStatusEffectPool.Instantiate(requiredType);
		}

		public static IReadOnlyStatusEffect GetEmpty(EnumStatusType requiredType)
		{
			return new EmptyStatusEffect(requiredType);
		}
	}
}