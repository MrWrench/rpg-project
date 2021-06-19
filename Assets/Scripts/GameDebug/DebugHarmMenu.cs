using System;
using System.Linq;
using JetBrains.Annotations;
using RapidGUI;
using RapidGUI.Example;
using StatusFX;
using UnityEngine;

namespace GameDebug
{
	public class DebugHarmMenu : ExampleBase
	{
		private struct StatusInfoPrototype
		{
			public StatusEffectType EffectType;
			public float Amount;
			public float Damage;
			public float Strength;

			public StatusEffectInfo ToStatusEffect()
			{
				return new StatusEffectInfo(Amount, Damage, Strength);
			}
		}
	
		private struct DamageInfoPrototype
		{
			public DamageType Type;
			public float HealthAmount;
			public float PoiseAmount;

			public DamageInfo ToDamageInfo()
			{
				return new DamageInfo(Type, HealthAmount, PoiseAmount);
			}
		}
	
		private Character[] _characters = null!;
		private int _targetIndex;
		private Character _currentTarget;
		protected override string title => "Debug Status Menu";

		#region Common

		private void Start()
		{
			OnCharactersChanged();
			if (_characters.Length > _targetIndex)
				_currentTarget = _characters[_targetIndex];
			CharacterDebug.OnSpawn += character => OnCharactersChanged();
			CharacterDebug.OnEnabled += character => OnCharactersChanged();
			CharacterDebug.OnDisabled += character => OnCharactersChanged();
			CharacterDebug.OnDestroyed += OnCharactersChanged;
		}

		private void OnCharactersChanged()
		{
			_characters = FindObjectsOfType<Character>();
			Array.Reverse(_characters);
			_targetIndex = _currentTarget != null ? Array.IndexOf(_characters, _currentTarget) : 0;
		}

		public override void DoGUI()
		{
			var textAlign = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
			using (new GUILayout.HorizontalScope())
			{
				if (GUILayout.Button("◀"))
					SwitchTarget(-1);
				GUILayout.FlexibleSpace();
				var targetName = _currentTarget != null ? _currentTarget.gameObject.name : "null";
				GUILayout.Label($"<b>{targetName}</b>", textAlign);
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("▶"))
					SwitchTarget(1);
			}
		
			if (_currentTarget == null)
				return;
			GUILayout.Label($"Health: {_currentTarget.Health:N1}", textAlign);
			GUILayout.Label($"Poise: {_currentTarget.Poise:N1}", textAlign);

			DrawStatuses(_currentTarget);
			DrawApplyStatus(_currentTarget);
			DrawDamage(_currentTarget);
		}

		void SwitchTarget(int delta)
		{
			_targetIndex = (_targetIndex + delta) % _characters.Length;
			if (_targetIndex < 0)
				_targetIndex = Math.Max(_characters.Length - 1, 0);

			if (_targetIndex < _characters.Length)
				_currentTarget = _characters[_targetIndex];
		}

		#endregion

		#region Status

		private bool _statusesUnfolded;
		private Vector2 _statusScrollPos;
		private void DrawStatuses([NotNull] Character target)
		{
			if (target == null) 
				throw new ArgumentNullException(nameof(target));
		
			_statusesUnfolded = Fold.DoGUIHeader(_statusesUnfolded, "Statuses");
			if (!_statusesUnfolded) return;

			using (var scrollView = new GUILayout.ScrollViewScope(_statusScrollPos))
			{
				_statusScrollPos = scrollView.scrollPosition;
				using (new RGUI.IndentScope())
				{
					using (new GUILayout.HorizontalScope())
					{
						GUILayout.Label("status");
						GUILayout.FlexibleSpace();
						GUILayout.Label("amount");
						GUILayout.FlexibleSpace();
						GUILayout.Label("damage");
						GUILayout.FlexibleSpace();
						GUILayout.Label("strength");
					}
					foreach (var gauge in target.StatusFX.AsEnumerable().OfType<IGaugeStatusEffect>())
					{
						using (new GUILayout.HorizontalScope())
						{
							GUILayout.Label(gauge.EffectType.ToString());
							GUILayout.FlexibleSpace();
							GUILayout.Label(gauge.Amount.ToString("P"));
							GUILayout.FlexibleSpace();
							GUILayout.Label(gauge.Damage.ToString("N"));
							GUILayout.FlexibleSpace();
							GUILayout.Label(gauge.Strength.ToString("N"));
						}
					}
				}
			}
		}

		private bool _statusInfoUnfolded;
		private StatusInfoPrototype _statusPrototype;
		private void DrawApplyStatus([NotNull] Character target)
		{
			if (target == null) 
				throw new ArgumentNullException(nameof(target));
		
			_statusInfoUnfolded = Fold.DoGUIHeader(_statusInfoUnfolded, "Apply Status");
			if (!_statusInfoUnfolded) return;

			using (new RGUI.IndentScope())
			{
				_statusPrototype.EffectType = RGUI.Field(_statusPrototype.EffectType, "Status");
				_statusPrototype.Amount = RGUI.Slider(_statusPrototype.Amount, 0f, 1f, "Amount");
				_statusPrototype.Damage = RGUI.Field(_statusPrototype.Damage, "Damage");
				_statusPrototype.Strength = RGUI.Field(_statusPrototype.Strength, "Strength");

				using (new GUILayout.HorizontalScope())
				{
					if (GUILayout.Button("Add Status"))
						target.ApplyStatusEffect(_statusPrototype.EffectType, _statusPrototype.ToStatusEffect());
					if (GUILayout.Button("Clear Status"))
						target.ClearStatusEffect(_statusPrototype.EffectType);
				}
			}
		}

		#endregion

		#region Damage

		private bool _damageUnfolded;
		private DamageInfoPrototype _damagePrototype;
		private void DrawDamage([NotNull] Character target)
		{
			if (target == null) 
				throw new ArgumentNullException(nameof(target));
		
			_damageUnfolded = Fold.DoGUIHeader(_damageUnfolded, "Apply Damage");
			if (!_damageUnfolded) return;
			using (new RGUI.IndentScope())
			{
				_damagePrototype = RGUI.Field(_damagePrototype, "Damage");

				if (GUILayout.Button("Apply Damage"))
					target.TakeDamage(_damagePrototype.ToDamageInfo());
			}
		}

		#endregion

	}
}