using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StatusFX
{
	[FilePath("Configs/StatusFX.asset", FilePathAttribute.Location.PreferencesFolder)]
	[CreateAssetMenu(fileName = "StatusFX", menuName = "StatusFX/StatusFXDefaults", order = 1)]
	public class StatusFXDefaults : ScriptableSingleton<StatusFXDefaults>
	{
		public IEnumerable<StatusEffect> DefaultEffects => _defaultEffects;
		[SerializeField] private List<StatusEffect> _defaultEffects = new List<StatusEffect>();
	}
}