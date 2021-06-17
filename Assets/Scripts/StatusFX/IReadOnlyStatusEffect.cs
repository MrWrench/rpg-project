namespace StatusFX
{
	public interface IReadOnlyStatusEffect
	{
		StatusEffectType EffectType { get; }
		bool IsDebuff { get; }
		bool IsStarted { get; }
	}
}