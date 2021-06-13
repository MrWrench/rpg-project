using System;
using JetBrains.Annotations;

namespace StatusFX
{
  public abstract class BaseStatusFX : IStatusEffect
  {
    protected readonly Character target;
    public bool started { get; private set; }
    public abstract EnumStatusType statusType { get; }
    public abstract bool isDebuff { get; }
    
    public event IStatusEffect.StartDelegate? onStarted;
    public event IStatusEffect.StopDelegate? onStoped;

    protected BaseStatusFX([NotNull] Character target)
    {
      this.target = target != null ? target : throw new ArgumentNullException(nameof(target));
    }

    public virtual void Update()
    {
      OnUpdate();
    }

    protected virtual void OnUpdate() { }

    public void Start()
    {
      started = true;
      OnStart();
      onStarted?.Invoke(this);
    }
    protected virtual void OnStart() { }
  
    public void Stop()
    {
      started = false;
      OnStop();
      onStoped?.Invoke(this);
    }

    protected virtual void OnStop() { }
  }
}