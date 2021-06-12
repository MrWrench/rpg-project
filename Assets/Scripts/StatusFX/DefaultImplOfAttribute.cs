using System;

namespace StatusFX
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public sealed class DefaultImplOfAttribute : Attribute
  {
    public readonly EnumStatusType status;

    public DefaultImplOfAttribute(EnumStatusType status)
    {
      this.status = status;
    }
  }
}