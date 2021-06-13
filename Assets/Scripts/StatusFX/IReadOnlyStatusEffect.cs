namespace StatusFX
{
	public interface IReadOnlyStatusEffect
	{
		EnumStatusType type { get; }
		bool isStarted { get; }
		bool isDebuff { get; }
	}
}