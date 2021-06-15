namespace StatusFX
{
	public static class StatusEffect
	{
		public static IStatusEffect GetDefault(EnumStatusType requiredType)
		{
			return DefaultStatusEffectPool.Instantiate(requiredType);
		}

		public static IReadOnlyStatusEffect GetEmpty(EnumStatusType requiredType)
		{
			return new EmptyStatusEffect(requiredType);
		}
	}
}