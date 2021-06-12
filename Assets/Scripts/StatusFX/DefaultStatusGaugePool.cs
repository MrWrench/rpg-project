using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace StatusFX
{
  public static class DefaultStatusGaugePool
  {
    private static readonly Dictionary<EnumStatusType, Type> defaultStatuses;

    static DefaultStatusGaugePool()
    {
      var gaugeType = typeof(BaseGaugeStatusFX);
      defaultStatuses = gaugeType
        .Assembly.GetTypes()
        .Select(type => (attr: (DefaultStatusFX) type.GetCustomAttribute(typeof(DefaultStatusFX)),
          type: type))
        .Where(tuple => tuple.type.IsSubclassOf(gaugeType) && !tuple.type.IsAbstract && tuple.attr != null)
        .ToDictionary(tuple => tuple.attr.status, tuple => tuple.type);
    }

    public static BaseGaugeStatusFX Instantiate(EnumStatusType status_type, [NotNull] Character character)
    {
      if (character == null) throw new ArgumentNullException(nameof(character));
      
      if (!defaultStatuses.ContainsKey(status_type))
        throw new ArgumentException($"Default status effect for type {status_type} does not exist");

      var type = defaultStatuses[status_type];
      return (BaseGaugeStatusFX) Activator.CreateInstance(type, args: character);
    }
  }
}