using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StatusFX
{
	[FilePath("Configs/StatusFX.asset", FilePathAttribute.Location.PreferencesFolder)]
	public class StatusFXDefaults : ScriptableSingleton<StatusFXDefaults>
	{
		public IEnumerable<IStatusEffectConfig> DefaultEffects => _defaultEffects;
		[SerializeField] private List<StatusEffectConfig> _defaultEffects = new List<StatusEffectConfig>();
	}
}