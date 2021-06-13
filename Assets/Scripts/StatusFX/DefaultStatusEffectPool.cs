using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StatusFX
{
  internal static class DefaultStatusEffectPool
  {
    private static readonly Dictionary<EnumStatusType, Type> defaultStatusFX;

    static DefaultStatusEffectPool()
    {
      var statusEffectType = typeof(IStatusEffect);
      defaultStatusFX = statusEffectType.Assembly.GetTypes()
        .Select(type => (attr: type.GetCustomAttribute<DefaultStatusEffectAttribute>(), type: type))
        .Where(tuple => statusEffectType.IsAssignableFrom(tuple.type) && !tuple.type.IsAbstract && tuple.attr != null)
        .ToDictionary(tuple => tuple.attr.type, tuple => tuple.type);
    }

    public static IStatusEffect Instantiate(EnumStatusType statusType)
    {
      if (!defaultStatusFX.ContainsKey(statusType))
        throw new ArgumentException($"Default status effect for type {statusType} does not exist");

      var type = defaultStatusFX[statusType];
      return (IStatusEffect) Activator.CreateInstance(type);
    }

    public static Type? FindDefaultType(EnumStatusType statusType)
    {
      return defaultStatusFX.TryGetValue(statusType, out var result) ? result : null;
    }
  }
}