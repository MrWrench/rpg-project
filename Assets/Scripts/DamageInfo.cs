using System;

public struct DamageInfo
{
	public EnumDamageType type;
	public float healthAmount;
	public float poiseAmount;

	public DamageInfo(EnumDamageType type, float healthAmount, float poiseAmount = 0)
	{
		if (healthAmount <= 0) throw new ArgumentOutOfRangeException(nameof(healthAmount));
		if (poiseAmount < 0) throw new ArgumentOutOfRangeException(nameof(poiseAmount));

		this.type = type;
		this.healthAmount = healthAmount;
		this.poiseAmount = poiseAmount;
	}
}