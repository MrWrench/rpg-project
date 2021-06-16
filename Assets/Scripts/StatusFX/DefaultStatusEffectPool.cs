using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StatusFX
{
  internal static class DefaultStatusEffectPool
  {
    private static readonly Dictionary<StatusEffectType, Type> defaultStatusFX;

    static DefaultStatusEffectPool()
    {
      var statusEffectType = typeof(IStatusEffect);
      defaultStatusFX = statusEffectType.Assembly.GetTypes()
        .Select(type => (attr: type.GetCustomAttribute<DefaultStatusEffectAttribute>(), type: type))
        .Where(tuple => statusEffectType.IsAssignableFrom(tuple.type) && !tuple.type.IsAbstract && tuple.attr != null)
        .ToDictionary(tuple => tuple.attr.EffectType, tuple => tuple.type);
    }

    public static IStatusEffect Instantiate(StatusEffectType statusEffectType)
    {
      if (!defaultStatusFX.ContainsKey(statusEffectType))
        throw new ArgumentException($"Default status effect for type {statusEffectType} does not exist");

      var type = defaultStatusFX[statusEffectType];
      return (IStatusEffect) Activator.CreateInstance(type);
    }

    public static Type? FindDefaultType(StatusEffectType statusEffectType)
    {
      return defaultStatusFX.TryGetValue(statusEffectType, out var result) ? result : null;
    }
  }
}