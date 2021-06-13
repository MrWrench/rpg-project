using System;
using UnityEngine;

namespace StatusFX
{
  public abstract class GaugeStatusEffect : StatusEffect, IGaugeStatusEffect
  {
    public float amount { get; private set; }
    public float strength { get; private set; }
    public float damage { get; private set; }
    public float baseDecayRate => 0.1f;
  
    protected float decayRate => baseDecayRate / (1 + target.debuffDurationMult);
  
    protected GaugeStatusEffect(Character target) : base(target) { }

    public void Add(StatusEffectInfo effectInfo, float factor = 1)
    {
      if (effectInfo.amount > 1 || effectInfo.amount < 0)
        throw new ArgumentOutOfRangeException($"{nameof(effectInfo)}.{nameof(effectInfo.amount)}");
      
      if (effectInfo.damage < 0)
        throw new ArgumentOutOfRangeException($"{nameof(effectInfo)}.{nameof(effectInfo.damage)}");
      
      if (effectInfo.strength < 0)
        throw new ArgumentOutOfRangeException($"{nameof(effectInfo)}.{nameof(effectInfo.strength)}");
      
      if (factor <= 0)
        throw new ArgumentOutOfRangeException(nameof(factor));

      if (isStarted)
        return;

      var addedAmount = Mathf.Min(1 - amount, effectInfo.amount * factor);
      if(addedAmount > 0)
      {
        strength = (strength * amount + effectInfo.strength * addedAmount) / (amount + addedAmount);
        damage = (damage * amount + effectInfo.damage * addedAmount) / (amount + addedAmount);
        amount += addedAmount;
      }

      if (amount >= 1)
        Trigger();
    }

    public void Clear()
    {
      Exhaust();
    }

    protected sealed override void Update()
    {
      if (amount > 0)
      {
        amount -= decayRate * Time.deltaTime;

        if (amount <= 0)
        {
          Exhaust();
        }
      }
      base.Update();
    }

    private void Trigger()
    {
      amount = 1;
      Start();
    }

    private void Exhaust()
    {
      amount = 0;
      Stop();
    }

    public static IReadOnlyGaugeStatusEffect GetEmpty(EnumStatusType requiredType)
    {
      return new EmptyGaugeStatusEffect(requiredType, false);
    }
  }
}