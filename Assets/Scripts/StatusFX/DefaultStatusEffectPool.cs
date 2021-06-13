using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace StatusFX
{
  internal static class DefaultStatusEffectPool
  {
    private static readonly Dictionary<EnumStatusType, Type> defaultStatusFX;

    static DefaultStatusEffectPool()
    {
      var statusEffectType = typeof(IStatusEffect);
      defaultStatusFX = statusEffectType
        .Assembly.GetTypes()
        .Select(type => (attr: (DefaultStatusFX) type.GetCustomAttribute(typeof(DefaultStatusFX)),
          type: type))
        .Where(tuple => tuple.type.IsSubclassOf(statusEffectType) && !tuple.type.IsAbstract && tuple.attr != null)
        .ToDictionary(tuple => tuple.attr.status, tuple => tuple.type);
    }

    public static IStatusEffect Instantiate(EnumStatusType statusType, [NotNull] Character character)
    {
      if (character == null)
        throw new ArgumentNullException(nameof(character));
      
      if (!defaultStatusFX.ContainsKey(statusType))
        throw new ArgumentException($"Default status effect for type {statusType} does not exist");

      var type = defaultStatusFX[statusType];
      return (IStatusEffect) Activator.CreateInstance(type, args: character);
    }
  }
}