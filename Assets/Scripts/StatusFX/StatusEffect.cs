using System;
using JetBrains.Annotations;
using UniRx;
using UniRx.Triggers;

namespace StatusFX
{
  public abstract class StatusEffect : IStatusEffect
  {
    protected readonly Character target;
    public bool isStarted { get; private set; }
    public abstract EnumStatusType type { get; }
    public abstract bool isDebuff { get; }
    
    public event IStatusEffect.StartDelegate? onStarted;
    public event IStatusEffect.StopDelegate? onStoped;

    protected StatusEffect([NotNull] Character target)
    {
      this.target = target != null ? target : throw new ArgumentNullException(nameof(target));
      this.target.UpdateAsObservable().Subscribe(_ => Update());
    }

    protected virtual void Update()
    {
      OnUpdate();
    }

    protected virtual void OnUpdate() { }

    public void Start()
    {
      isStarted = true;
      OnStart();
      onStarted?.Invoke(this);
    }
    protected virtual void OnStart() { }
  
    public void Stop()
    {
      isStarted = false;
      OnStop();
      onStoped?.Invoke(this);
    }

    protected virtual void OnStop() { }

    public IStatusEffect GetDefault(EnumStatusType requiredType, Character character)
    {
      return DefaultStatusEffectPool.Instantiate(requiredType, character);
    }
  }
}