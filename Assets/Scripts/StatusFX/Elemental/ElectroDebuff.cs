using JetBrains.Annotations;
using UnityEngine;

namespace StatusFX
{
	public class ElectroDebuff : BaseGaugeStatusFX
	{
		// TODO: move to config
		private const float MAX_DISCHARGE_TIME = 3;
		private const float MIN_DISCHARGE_TIME = 1;
		private const float DISCHARGE_RADIUS = 5;
		public override EnumStatusType statusType => EnumStatusType.ELECTRO;
		private float accumulatedDamage;
		private float nextDischargeTime;

		public ElectroDebuff([NotNull] Character target) : base(target)
		{
			target.onTakeDamage += OnTakeDamage;
			Discharge();
		}

		protected override void OnUpdate()
		{
			if(Time.time < nextDischargeTime)
				return;
			
			Discharge();
		}

		protected override void OnStop()
		{
			accumulatedDamage = 0;
		}

		private void OnTakeDamage(DamageInfo info, float factor)
		{
			if(!started)
				return;
			
			accumulatedDamage += info.healthAmount * factor;
		}

		private void Discharge()
		{
			nextDischargeTime = GetNextDischargeTime();
			
			if(accumulatedDamage <= 0)
				return;

			var colliders = Physics.OverlapSphere(target.transform.position, DISCHARGE_RADIUS);
			
			if(colliders.Length <= 0)
				return;
			
			foreach (var collider in colliders)
			{
				var victim = collider.GetComponent<Character>();
				if(victim == null || victim == target)
					return;
				
				victim.TakeDamage(new DamageInfo(EnumDamageType.ELEMENTAL, accumulatedDamage * strength));
			}

			accumulatedDamage = 0;
		}
		
		private static float GetNextDischargeTime()
		{
			return Random.Range(MIN_DISCHARGE_TIME, MAX_DISCHARGE_TIME);
		}
	}
}