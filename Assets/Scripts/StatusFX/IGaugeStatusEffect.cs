namespace StatusFX
{
	public interface IGaugeStatusEffect : IStatusEffect
	{
		float amount { get; }
		float damage { get; }
		float strength { get; }
	}
}