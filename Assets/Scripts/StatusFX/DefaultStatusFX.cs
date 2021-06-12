using System;

namespace StatusFX
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public sealed class DefaultStatusFX : Attribute
  {
    public readonly EnumStatusType status;

    public DefaultStatusFX(EnumStatusType status)
    {
      this.status = status;
    }
  }
}