namespace StatusFX
{
	public interface IStatusEffect : IReadOnlyStatusEffect
	{
		delegate void StateChangeDelegate(IStatusEffect statusEffect);
		event StateChangeDelegate OnStarted;
		event StateChangeDelegate OnStopped;

		void Start();
		void Stop();
		
		void LinkNewTarget(IStatusFXCarrier newTarget);
		void UnlinkCurrentTarget();

		IStatusEffect CreateInstance();
	}
}