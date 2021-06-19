namespace StatusFX
{
	public interface IStatusFXCarrier : IUpdateProvider
	{
		IStatusCollection StatusFX { get; }
		void ApplyStatusEffect(StatusEffectType effectType, StatusEffectInfo info, float factor = 1);
		void ClearStatusEffect(StatusEffectType effectType);
		
		void ImplementStatusEffect(IStatusEffect statusEffect);
		void ReimplementStatusEffect(IStatusEffect statusEffect);
	}
}