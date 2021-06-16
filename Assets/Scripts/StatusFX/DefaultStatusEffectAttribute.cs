using System;

namespace StatusFX
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public sealed class DefaultStatusEffectAttribute : Attribute
  {
    public readonly StatusEffectType EffectType;
    public readonly bool IsDebuff;

    public DefaultStatusEffectAttribute(StatusEffectType effectType, bool isDebuff)
    {
      this.EffectType = effectType;
      this.IsDebuff = isDebuff;
    }
  }
}