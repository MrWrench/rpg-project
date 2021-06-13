namespace StatusFX
{
	public interface IGaugeStatusEffect : IStatusEffect, IReadOnlyGaugeStatusEffect
	{
		void Add(StatusEffectInfo effectInfo, float factor = 1);
		void Clear();
	}
}