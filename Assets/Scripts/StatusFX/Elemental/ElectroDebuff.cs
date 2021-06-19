using System.Linq;
using UnityEngine;

namespace StatusFX.Elemental
{
	[CreateAssetMenu(fileName = "Electro Debuff", menuName = "StatusFX/Electro Debuff", order = 2)]
	internal sealed class ElectroDebuff : ElementalDebuff
	{
		[SerializeField] private float _maxDischargeTime = 3;
		[SerializeField] private float _minDischargeTime = 1;
		[SerializeField] private float _dischargeRadius = 5;
		[SerializeField] private float _dischargePoiseDamage = 10;
		[SerializeField] private float _dischargeDamageMult = 0.3f;
		[SerializeField] private float _dischargeAccumulatedDamageMult = 0.7f;
		[SerializeField] private float _statusSpreadMult = 0.5f;
		[SerializeField] private float _statusSpreadMax = 0.7f;

		public float MaxDischargeTime => _maxDischargeTime;
		public float MinDischargeTime => _minDischargeTime;
		public float DischargeRadius => _dischargeRadius;
		public float DischargePoiseDamage => _dischargePoiseDamage;
		public float DischargeDamageMult => _dischargeDamageMult;
		public float DischargeAccumulatedDamageMult => _dischargeAccumulatedDamageMult;
		public float StatusSpreadMult => _statusSpreadMult;
		public float StatusSpreadMax => _statusSpreadMax;
		
		private float _accumulatedDamage;
		private float _nextDischargeTime;

		protected override void OnStart()
		{
			Target.OnTakeDamage += OnTakeDamage;
		}

		protected override void OnUpdate()
		{
			if (!IsStarted || Time.time < _nextDischargeTime)
				return;

			Discharge();
		}

		protected override void OnStop()
		{
			Target.OnTakeDamage -= OnTakeDamage;
			_accumulatedDamage = 0;
			_nextDischargeTime = 0;
		}

		private void OnTakeDamage(DamageInfo info, float factor)
		{
			if (!IsStarted)
				return;

			_accumulatedDamage += info.HealthAmount * factor;
		}

		private void Discharge()
		{
			_nextDischargeTime = GetNextDischargeTime();

			var dischargeDamage = GetDischargeTotalDamage();
			var dischargePoiseDamage = GetDischargePoiseDamage();
			Target.TakeDamage(new DamageInfo(DamageType.Elemental, dischargeDamage, dischargePoiseDamage));
			_accumulatedDamage -= dischargeDamage;

			var victims = StatusEffectsQueries.FindFriendsInSphere(Target, Target.GetTransform().position, DischargeRadius);
			if (victims.Count > 0)
			{
				var victimCount = victims.Count + 1; // Себя учитываем тоже
				var dischargeSingleDamage = dischargeDamage / victimCount;

				var appliedStatuses = Target.StatusFX.AsEnumerable()
					.OfType<IGaugeStatusEffect>()
					.Where(x => x.IsStarted)
					.Select(x => (statusType: x!.EffectType, statusStats:
						new StatusEffectInfo(
							Mathf.Min(StatusSpreadMax, x.Amount * StatusSpreadMult * Strength),
							x.Damage / victimCount,
							x.Strength / victimCount))).ToList();

				foreach (var victim in victims)
				{
					victim.TakeDamage(new DamageInfo(DamageType.Elemental, dischargeSingleDamage,
						dischargePoiseDamage));

					foreach (var statusInfo in appliedStatuses)
						victim.ApplyStatusEffect(statusInfo.statusType, statusInfo.statusStats);
				}
			}

			_accumulatedDamage = 0;
		}

		private float GetDischargePoiseDamage()
		{
			return DischargePoiseDamage * Strength;
		}

		private float GetDischargeTotalDamage()
		{
			var maxDamage = _accumulatedDamage + Damage;
			var calculatedDamage = _accumulatedDamage * DischargeAccumulatedDamageMult * Strength
			                       + Damage * DischargeDamageMult;
			return Mathf.Min(calculatedDamage, maxDamage);
		}

		private float GetNextDischargeTime()
		{
			return Time.time + Random.Range(MinDischargeTime, MaxDischargeTime);
		}
	}
}