namespace StatusFX
{
	public interface IStatusFXCarrier : IUpdateProvider
	{
		IStatusCollection statusFX { get; }
		void ApplyStatus(EnumStatusType type, StatusEffectInfo info, float factor = 1);
		void ClearStatus(EnumStatusType type);
	}
}