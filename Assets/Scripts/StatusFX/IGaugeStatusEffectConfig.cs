namespace StatusFX
{
	public interface IGaugeStatusEffectConfig : IStatusEffectConfig
	{
		float GetBaseDecayRate { get; }
	}
}