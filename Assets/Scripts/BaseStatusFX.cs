public abstract class BaseStatusFX
{
  private Character target;
  protected bool started { get; private set; } = false;

  protected BaseStatusFX(Character target)
  {
    this.target = target;
  }

  public void Update()
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