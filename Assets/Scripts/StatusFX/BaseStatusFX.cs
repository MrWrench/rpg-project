namespace StatusFX
{
  public abstract class BaseStatusFX
  {
    protected readonly Character target;
    public bool started { get; private set; } = false;
    public abstract EnumStatusType statusType { get; }

    protected BaseStatusFX(Character target)
    {
      this.target = target;
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
    }
    protected virtual void OnStart() { }
  
    public void Stop()
    {
      started = false;
      OnStop();
    }
  
    protected virtual void OnStop() { }
  }
}