using System.Linq;
using Game;
using UnityEngine;

namespace StatusFX.Components
{
	public class ElectroComponent : StatusComponent
	{
		public float DischargeDamageMult { get; set; }
		public float AccumulatedDamage { get; private set; }
		public float DischargeRadius { get; set; }
		public float DischargePeriod { get; set; }
		private float _nextDischargeStamp;

		protected override void OnAdded()
		{
			Owner.Target.OnDamageTaken += OnTargetDamageTaken;
		}

		protected override void OnRemoved(Wrenge.StatusFX.StatusEffect previousOwner)
		{
			Owner.Target.OnDamageTaken -= OnTargetDamageTaken;
		}

		private void OnTargetDamageTaken(DamageInfo info)
		{
			if (!IsActive ||
			    info.Inflictor is StatusEffect status && status.Tag == Owner.Tag)
				return;

			AccumulatedDamage += info.HealthAmount;
		}

		private void Discharge()
		{
			var collisions =
				Physics.OverlapSphere(Owner.Target.transform.position, DischargeRadius, LayerMask.GetMask("Default"))
					.Select(x => x.GetComponent<Character>())
					.Where(x => x != null && x.Team == Owner.Target.Team);

			var damage = Owner.Damage * DischargeDamageMult * Owner.CurrentStacks + AccumulatedDamage * Owner.Strength;
			var damageInfo = new DamageInfo {HealthAmount = damage, Type = DamageType.Elemental, Inflictor = Owner};
			foreach (var character in collisions)
			{
				character.ApplyDamage(damageInfo);
			}

			_nextDischargeStamp = Time.CurrentTime + DischargePeriod;
		}

		protected override void OnStacksChanged(int deltaStacks)
		{
			if (Owner.CurrentStacks == 0)
				_nextDischargeStamp = 0;
		}

		public override void Tick()
		{
			if (_nextDischargeStamp > Time.CurrentTime)
				return;

			Discharge();
		}
	}
}