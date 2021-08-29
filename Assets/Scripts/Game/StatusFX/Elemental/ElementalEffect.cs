using Game;
using StatusFX.Components;

namespace StatusFX.Elemental
{
	public abstract class ElementalEffect : StatusEffect
	{
		private static float BaseDecayRate => 0.3f;
		
		public ElementalEffect(Character target) : base(target)
		{
			AddComponent(new AccumulationComponent() {BaseDecayRate = BaseDecayRate});
			AddComponent(new ExpirationComponent() {Duration = 3});
		}
	}
}