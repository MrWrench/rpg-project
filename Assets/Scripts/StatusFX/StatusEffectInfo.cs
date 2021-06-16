using System;

namespace StatusFX
{
  public readonly struct StatusEffectInfo
  {
    public readonly float Amount;
    public readonly float Damage;
    public readonly float Strength;

    public StatusEffectInfo(float amount, float damage = 0, float strength = 0)
    {
      if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
      if (damage < 0) throw new ArgumentOutOfRangeException(nameof(damage));
      if (damage == 0 && strength == 0) throw new ArgumentOutOfRangeException($"{nameof(damage)} or {nameof(amount)}");
      Amount = amount;
      Damage = damage;
      Strength = strength;
    }
  }
}