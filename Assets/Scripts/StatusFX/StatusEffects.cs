using System.Collections.Generic;

namespace StatusFX
{
	public class StatusEffects
	{
		public IReadOnlyList<StatusEffect> Effects => _effectsList;
		public int Count => _effectsList.Count;
		private readonly List<StatusEffect> _effectsList = new List<StatusEffect>();
		private readonly Dictionary<StatusTag, StatusEffect> _effectsDict = new Dictionary<StatusTag, StatusEffect>();

		public void Tick()
		{
			foreach (var statusEffect in _effectsList)
			{
				if(statusEffect.CurrentStacks <= 0)
					continue;
				
				statusEffect.Tick();
			}
		}

		public void Add(StatusEffect effect)
		{
			if(_effectsDict.ContainsKey(effect.StatusTag))
				return;
			
			_effectsDict.Add(effect.StatusTag, effect);
			_effectsList.Add(effect);
		}

		public StatusEffect Get(int i) => _effectsList[i];
		
		public StatusEffect Get(StatusTag tag) => _effectsDict[tag];

		public bool Has(StatusTag tag) => _effectsDict.ContainsKey(tag);
	}
}