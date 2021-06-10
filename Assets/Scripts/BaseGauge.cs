using System;
using UnityEngine;

public abstract class BaseGauge
{
  public float gauge { get; private set; } = 0;
  public float strength { get; private set; } = 0;
  public float damage { get; private set; } = 0;
  public bool triggered { get; private set; } = false;
  public float baseDecayRate => 0.1f;
  public event Action<BaseGauge> onTriggered;

  protected float decayRate => baseDecayRate;

  private Character owner;

  protected BaseGauge(Character owner)
  {
    this.owner = owner;
  }

  public void Add(float newGauge, float newStrength, float newDamage)
  {
    if (triggered)
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

  private void Update()
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
  
  protected virtual void OnUpdate() { }

  private void Trigger()
  {
    gauge = 1;
    triggered = true;
    OnTrigger();
    onTriggered?.Invoke(this);
  }

  protected virtual void OnTrigger()
  {
  }

  private void Exhaust()
  {
    gauge = 0;
    triggered = false;
    OnExhaust();
  }

  protected virtual void OnExhaust()
  {
  }
}