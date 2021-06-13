using System;

public readonly struct DamageInfo
{
	public readonly EnumDamageType type;
	public readonly float healthAmount;
	public readonly float poiseAmount;

	public DamageInfo(EnumDamageType type, float healthAmount, float poiseAmount = 0)
	{
		if (healthAmount < 0) throw new ArgumentOutOfRangeException(nameof(healthAmount));
		if (poiseAmount < 0) throw new ArgumentOutOfRangeException(nameof(poiseAmount));
		if (healthAmount == 0 && poiseAmount == 0) throw new ArgumentOutOfRangeException($"{nameof(healthAmount)} or {nameof(poiseAmount)}");

		this.type = type;
		this.healthAmount = healthAmount;
		this.poiseAmount = poiseAmount;
	}
}