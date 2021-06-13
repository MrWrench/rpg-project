namespace StatusFX
{
	public interface IStatusEffect : IReadOnlyStatusEffect
	{
		delegate void StartDelegate(IStatusEffect statusEffect);
		event StartDelegate onStarted;

		delegate void StopDelegate(IStatusEffect statusEffect);
		event StopDelegate onStoped;

		void Start();
		void Stop();

		void LinkNewTarget(IStatusFXCarrier newTarget);
		void UnlinkCurrentTarget();
	}
}