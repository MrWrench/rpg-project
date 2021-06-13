namespace StatusFX
{
	public interface IReadOnlyGaugeStatusEffect : IReadOnlyStatusEffect
	{
		float amount { get; }
		float damage { get; }
		float strength { get; }
	}
}