using UnityEngine;

namespace StatusFX.Elemental.Configs
{
	[CreateAssetMenu(fileName = "Fire Debuff Config", menuName = "Debuffs/Fire", order = 0)]
	internal class FireDebuffConfig : GaugeStatusEffectConfig<FireDebuff>
	{
		[SerializeField] private float _explosionRadius = 5;
    [SerializeField] private float _statusSpreadMult = 0.4f;
    [SerializeField] private float _explosionDamageMult = 0.4f;
    [SerializeField] private float _explosionStrengthMult = 0.4f;
    [SerializeField] private float _explosionPoiseDamage = 40;
    [SerializeField] private float _hydroStrengthMult = 0.5f;

    public float ExplosionRadius => _explosionRadius;
    public float StatusSpreadMult => _statusSpreadMult;
    public float ExplosionDamageMult => _explosionDamageMult;
    public float ExplosionStrengthMult => _explosionStrengthMult;
    public float ExplosionPoiseDamage => _explosionPoiseDamage;
    public float HydroStrengthMult => _hydroStrengthMult;
	}
}