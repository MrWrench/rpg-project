namespace Wrenge.StatusFX
{
	public interface ITimeProvider
	{
		float CurrentTime { get; }
		float DeltaTime { get; }
	}
}