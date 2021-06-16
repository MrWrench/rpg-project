public interface IDamageable
{
	delegate void TakeDamageDelegate(DamageInfo info, float factor);
	event TakeDamageDelegate OnTakeDamage;
	delegate void ApplyDamageDelegate(DamageInfo info);
	event ApplyDamageDelegate OnAppliedDamage;

	void TakeDamage(DamageInfo info, float factor = 1);
}