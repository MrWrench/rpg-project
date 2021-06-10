public abstract class BaseDebuff
{
  private Character target;

  protected BaseDebuff(Character target)
  {
    this.target = target;
  }

  public abstract void Start();
  public abstract void Stop();
}