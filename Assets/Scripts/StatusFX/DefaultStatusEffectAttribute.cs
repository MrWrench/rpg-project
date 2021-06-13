using System;

namespace StatusFX
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public sealed class DefaultStatusEffectAttribute : Attribute
  {
    public readonly EnumStatusType type;
    public readonly bool isDebuff;

    public DefaultStatusEffectAttribute(EnumStatusType type, bool isDebuff)
    {
      this.type = type;
      this.isDebuff = isDebuff;
    }
  }
}