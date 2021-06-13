using System;

namespace StatusFX
{
  public readonly struct StatusEffectInfo
  {
    public readonly float amount;
    public readonly float damage;
    public readonly float strength;

    public StatusEffectInfo(float amount, float damage = 0, float strength = 0)
    {
      if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
      if (damage < 0) throw new ArgumentOutOfRangeException(nameof(damage));
      if (damage == 0 && strength == 0) throw new ArgumentOutOfRangeException($"{nameof(damage)} or {nameof(amount)}");
      this.amount = amount;
      this.damage = damage;
      this.strength = strength;
    }
  }
}