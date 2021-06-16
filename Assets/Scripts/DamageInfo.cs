using System;

public readonly struct DamageInfo
{
	public readonly DamageType Type;
	public readonly float HealthAmount;
	public readonly float PoiseAmount;

	public DamageInfo(DamageType type, float healthAmount, float poiseAmount = 0)
	{
		if (healthAmount < 0) throw new ArgumentOutOfRangeException(nameof(healthAmount));
		if (poiseAmount < 0) throw new ArgumentOutOfRangeException(nameof(poiseAmount));
		if (healthAmount == 0 && poiseAmount == 0) throw new ArgumentOutOfRangeException($"{nameof(healthAmount)} or {nameof(poiseAmount)}");

		Type = type;
		HealthAmount = healthAmount;
		PoiseAmount = poiseAmount;
	}
}