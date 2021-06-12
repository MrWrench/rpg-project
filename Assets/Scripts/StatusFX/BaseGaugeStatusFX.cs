using System;
using UnityEngine;

namespace StatusFX
{
  public abstract class BaseGaugeStatusFX : BaseStatusFX
  {
    public float gauge { get; private set; } = 0;
    public float strength { get; private set; } = 0;
    public float damage { get; private set; } = 0;
    public float baseDecayRate => 0.1f;
  
    public event Action<BaseGaugeStatusFX>? onTriggered;

    protected float decayRate => baseDecayRate;
  
    protected BaseGaugeStatusFX(Character target) : base(target) { }
  
    public void Add(float newGauge, float newStrength, float newDamage)
    {
      if (started)
        return;

      newGauge = Mathf.Min(1 - gauge, newGauge);
      strength = (strength * gauge + newStrength * newGauge) / (gauge + newGauge);
      damage = (damage * gauge + newDamage * newGauge) / (gauge + newGauge);
      gauge += newGauge;

      if (gauge >= 1)
        Trigger();
    }

    public void Clear()
    {
      Exhaust();
    }

    public sealed override void Update()
    {
      if (gauge > 0)
      {
        gauge -= decayRate * Time.deltaTime;

        if (gauge <= 0)
        {
          Exhaust();
        }
      }
      OnUpdate();
    }

    private void Trigger()
    {
      gauge = 1;
      Start();
      onTriggered?.Invoke(this);
    }

    private void Exhaust()
    {
      gauge = 0;
      Stop();
    }
  }
}