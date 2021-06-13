namespace StatusFX.Generic
{
	public interface IStatusEffect<in T> : IStatusEffect where T : IStatusFXCarrier?
	{
		void LinkNewTarget(T newTarget);
	}
}