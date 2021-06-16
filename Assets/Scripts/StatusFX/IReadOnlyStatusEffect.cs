namespace StatusFX
{
	public interface IReadOnlyStatusEffect
	{
		StatusEffectType EffectType { get; }
		bool IsStarted { get; }
		bool IsDebuff { get; }
	}
}