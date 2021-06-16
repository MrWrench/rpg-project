namespace StatusFX
{
	public interface IStatusFXCarrier : IUpdateProvider
	{
		IStatusCollection StatusFX { get; }
		void ApplyStatus(StatusEffectType effectType, StatusEffectInfo info, float factor = 1);
		void ClearStatus(StatusEffectType effectType);
	}
}