using System.Reflection;

namespace StatusFX.Elemental
{
	[DefaultStatusEffect(EnumStatusType.HYDRO, true)]
	internal sealed class HydroDebuff : ElementalDebuff
	{
		private const float DEBUFF_AMOUNT = 0.5f;

		public override EnumStatusType type => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().type;

		public override bool isDebuff => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().isDebuff;

		private float appliedAmount;

		protected override void OnStart()
		{
			appliedAmount = DEBUFF_AMOUNT * strength;
			target.debuffDurationMult += appliedAmount;
		}

		protected override void OnStop()
		{
			target.debuffDurationMult -= appliedAmount;
			appliedAmount = 0;
		}
	}
}