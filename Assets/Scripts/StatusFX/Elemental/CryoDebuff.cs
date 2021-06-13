using System.Reflection;

namespace StatusFX.Elemental
{
	[DefaultStatusEffect(EnumStatusType.CRYO, true)]
	internal sealed class CryoDebuff : ElementalDebuff
	{
		private const float POISE_DEBUFF = 10;

		public override EnumStatusType type => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().type;

		public override bool isDebuff => GetType().GetCustomAttribute<DefaultStatusEffectAttribute>().isDebuff;

		private float appliedAmount;

		protected override void OnStart()
		{
			appliedAmount = POISE_DEBUFF * strength;
			target.poiseDamageDebuff += appliedAmount;
		}

		protected override void OnStop()
		{
			target.poiseDamageDebuff -= appliedAmount;
			appliedAmount = 0;
		}
	}
}