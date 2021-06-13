using System;

namespace StatusFX
{
  public struct AddStatusInfo
  {
    public float amount;
    public float damage;
    public float strength;

    public AddStatusInfo(float amount, float damage = 0, float strength = 0)
    {
      if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
      if (damage <= 0 && strength <= 0) throw new ArgumentOutOfRangeException($"{nameof(damage)} or {nameof(amount)}");
      this.amount = amount;
      this.damage = damage;
      this.strength = strength;
    }
  }
}