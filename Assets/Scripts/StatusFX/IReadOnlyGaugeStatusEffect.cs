namespace StatusFX
{
	public interface IReadOnlyGaugeStatusEffect : IReadOnlyStatusEffect
	{
		float Amount { get; }
		float Damage { get; }
		float Strength { get; }
	}
}