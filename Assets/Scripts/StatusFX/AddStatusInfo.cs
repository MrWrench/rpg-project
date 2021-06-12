namespace StatusFX
{
  public struct AddStatusInfo
  {
    public EnumStatusType status;
    public float amount;
    public float damage;
    public float strength;

    public AddStatusInfo(EnumStatusType status, float amount, float damage = 0, float strength = 0)
    {
      this.status = status;
      this.amount = amount;
      this.damage = damage;
      this.strength = strength;
    }
  }
}