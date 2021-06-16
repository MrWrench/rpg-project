using System;
using UnityEngine;

namespace StatusFX
{
	public abstract class StatusEffectConfig : ScriptableObject, IStatusEffectConfig
	{
		[SerializeField] private bool _isDebuff = false;
		[SerializeField] private StatusEffectType _effectType;
		public bool IsDebuff => _isDebuff;
		public StatusEffectType EffectType => _effectType;
		public abstract Type StatusEffectClassType { get; }
	}
}