using System;
using System.Collections.Generic;
using System.Linq;

namespace StatusFX
{
  internal static class DefaultStatusEffectPool
  {
    private static readonly Dictionary<StatusEffectType, IStatusEffectConfig> DefaultStatusFX;

    static DefaultStatusEffectPool()
    {
      var statusEffectType = typeof(IStatusEffect);
      DefaultStatusFX = StatusFXDefaults.instance.DefaultEffects.ToDictionary(x => x.EffectType, x => x);
    }

    internal static IStatusEffect Instantiate(StatusEffectType statusEffectType)
    {
      if (!DefaultStatusFX.ContainsKey(statusEffectType))
        throw new ArgumentException($"Default status effect for type {statusEffectType} does not exist");

      var config = DefaultStatusFX[statusEffectType];
      return Instantiate(config);
    }

    public static IStatusEffect Instantiate(IStatusEffectConfig config)
    {
      var type = config.StatusEffectClassType;
      var statusEffect = (IStatusEffect) Activator.CreateInstance(type);
      statusEffect.SetConfig(config);
      return statusEffect;
    }

    public static IStatusEffectConfig FindDefaultConfig(StatusEffectType statusEffectType)
    {
      return DefaultStatusFX.TryGetValue(statusEffectType, out var result) ? result : null;
    }
  }
}