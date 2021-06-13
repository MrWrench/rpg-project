using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using RapidGUI;
using RapidGUI.Example;
using StatusFX;
using UnityEngine;

public class DebugHarmMenu : ExampleBase
{
	private struct StatusInfoPrototype
	{
		public EnumStatusType type;
		public float amount;
		public float damage;
		public float strength;

		public StatusEffectInfo ToStatusEffect()
		{
			return new StatusEffectInfo(amount, damage, strength);
		}
	}
	
	private struct DamageInfoPrototype
	{
		public EnumDamageType type;
		public float healthAmount;
		public float poiseAmount;

		public DamageInfo ToDamageInfo()
		{
			return new DamageInfo(type, healthAmount, poiseAmount);
		}
	}
	
	private Character[] _characters = null!;
	private int _targetIndex;
	private Character? _currentTarget;
	protected override string title => "Debug Status Menu";

	#region Common

	private void Start()
	{
		OnCharactersChanged();
		if (_characters.Length > _targetIndex)
			_currentTarget = _characters[_targetIndex];
		Character.OnSpawn += character => OnCharactersChanged();
		Character.OnEnabled += character => OnCharactersChanged();
		Character.OnDisabled += character => OnCharactersChanged();
		Character.OnDestroyed += OnCharactersChanged;
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
		GUILayout.Label($"Health: {_currentTarget.health:N1}", textAlign);
		GUILayout.Label($"Poise: {_currentTarget.poise:N1}", textAlign);

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

		using (var scrollView =
			new GUILayout.ScrollViewScope(_statusScrollPos))
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
				foreach (var gauge in target.GetStatusEffects())
				{
					using (new GUILayout.HorizontalScope())
					{
						GUILayout.Label(gauge.statusType.ToString());
						GUILayout.FlexibleSpace();
						GUILayout.Label(gauge.amount.ToString("P"));
						GUILayout.FlexibleSpace();
						GUILayout.Label(gauge.damage.ToString("N"));
						GUILayout.FlexibleSpace();
						GUILayout.Label(gauge.strength.ToString("N"));
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
			_statusPrototype.type = RGUI.Field(_statusPrototype.type, "Status");
			_statusPrototype.amount = RGUI.Slider(_statusPrototype.amount, 0f, 1f, "Amount");
			_statusPrototype.damage = RGUI.Field(_statusPrototype.damage, "Damage");
			_statusPrototype.strength = RGUI.Field(_statusPrototype.strength, "Strength");

			using (new GUILayout.HorizontalScope())
			{
				if (GUILayout.Button("Add Status"))
					target.ApplyStatus(_statusPrototype.type, _statusPrototype.ToStatusEffect());
				if (GUILayout.Button("Clear Status"))
					target.ClearStatus(_statusPrototype.type);
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