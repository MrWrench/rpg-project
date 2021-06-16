namespace StatusFX
{
	public interface IStatusEffect : IReadOnlyStatusEffect
	{
		delegate void StartDelegate(IStatusEffect statusEffect);
		event StartDelegate OnStarted;

		delegate void StopDelegate(IStatusEffect statusEffect);
		event StopDelegate OnStopped;

		void Start();
		void Stop();

		void LinkNewTarget(IStatusFXCarrier newTarget);
		void UnlinkCurrentTarget();
	}
}