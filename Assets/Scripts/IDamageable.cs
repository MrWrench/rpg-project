public interface IDamageable
{
	delegate void TakeDamageDelegate(DamageInfo info, float factor);
	event TakeDamageDelegate? onTakeDamage;
	public delegate void ApplyDamageDelegate(DamageInfo info);
	event ApplyDamageDelegate? onAppliedDamage;

	void TakeDamage(DamageInfo info, float factor = 1);
}