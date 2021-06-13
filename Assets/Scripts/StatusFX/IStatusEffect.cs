namespace StatusFX
{
	public interface IStatusEffect
	{
		delegate void StartDelegate(IStatusEffect statusEffect);
		event StartDelegate onStarted;

		delegate void StopDelegate(IStatusEffect statusEffect);
		event StopDelegate onStoped;

		bool isDebuff { get; }
		void Start();
		void Stop();
	}
}