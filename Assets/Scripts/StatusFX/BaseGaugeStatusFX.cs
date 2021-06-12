using System;
using UnityEngine;

namespace StatusFX
{
  public abstract class BaseGaugeStatusFX : BaseStatusFX
  {
    public float amount { get; private set; } = 0;
    public float strength { get; private set; } = 0;
    public float damage { get; private set; } = 0;
    public float baseDecayRate => 0.1f;
  
    public event Action<BaseGaugeStatusFX>? onTriggered;

    protected float decayRate => baseDecayRate;
  
    protected BaseGaugeStatusFX(Character target) : base(target) { }

    public void Add(AddStatusInfo info)
    {
      if (info.amount > 1 || info.amount < 0)
        throw new ArgumentOutOfRangeException($"{nameof(info)}.{nameof(info.amount)}");
      
      if (info.damage < 0)
        throw new ArgumentOutOfRangeException($"{nameof(info)}.{nameof(info.damage)}");
      
      if (info.strength < 0)
        throw new ArgumentOutOfRangeException($"{nameof(info)}.{nameof(info.strength)}");
      
      if (started)
        return;

      var addedAmount = Mathf.Min(1 - amount, info.amount);
      strength = (strength * amount + info.strength * addedAmount) / (amount + addedAmount);
      damage = (damage * amount + info.damage * addedAmount) / (amount + addedAmount);
      amount += addedAmount;

      if (amount >= 1)
        Trigger();
    }

    public void Clear()
    {
      Exhaust();
    }

    public sealed override void Update()
    {
      if (amount > 0)
      {
        amount -= decayRate * Time.deltaTime;

        if (amount <= 0)
        {
          Exhaust();
        }
      }
      OnUpdate();
    }

    private void Trigger()
    {
      amount = 1;
      Start();
      onTriggered?.Invoke(this);
    }

    private void Exhaust()
    {
      amount = 0;
      Stop();
    }
  }
}