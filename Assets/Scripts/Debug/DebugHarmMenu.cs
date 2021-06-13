using System;
using System.Collections.Generic;
using RapidGUI;
using RapidGUI.Example;
using StatusFX;
using UnityEngine;

public class DebugHarmMenu : ExampleBase
{
	private Character[] _characters = null!;
	private int _targetIndex = 0;
	[SerializeField] private Character? target;
	private Vector2 _statusScrollPos;
	private AddStatusInfo _statusInfo = new AddStatusInfo();
	private DamageInfo _damageInfo = new DamageInfo();
	private string[] _enumStatusTypeValues = null!;
	private bool _statusesUnfolded = false;
	private bool _statusInfoUnfolded = false;
	private bool _damageUnfolded = false;

	private void Awake()
	{
		_enumStatusTypeValues = Enum.GetNames(typeof(EnumStatusType));
	}

	private void Start()
	{
		OnCharactersChanged();
		if (_characters.Length > _targetIndex)
			target = _characters[_targetIndex];
		Character.OnSpawn += character => OnCharactersChanged();
		Character.OnEnabled += character => OnCharactersChanged();
		Character.OnDisabled += character => OnCharactersChanged();
		Character.OnDestroyed += OnCharactersChanged;
	}

	private void OnCharactersChanged()
	{
		_characters = FindObjectsOfType<Character>();
		Array.Reverse(_characters);
		_targetIndex = target != null ? Array.IndexOf(_characters, target) : 0;
	}

	public override void DoGUI()
	{
		var textAlign = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
		using (new GUILayout.HorizontalScope())
		{
			if (GUILayout.Button("◀"))
				SwitchTarget(-1);
			GUILayout.FlexibleSpace();
			var targetName = target != null ? target.gameObject.name : "null";
			GUILayout.Label($"<b>{targetName}</b>", textAlign);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("▶"))
				SwitchTarget(1);
		}
		
		if (target == null)
			return;
		GUILayout.Label($"Health: {target.health:N1}", textAlign);
		GUILayout.Label($"Poise: {target.poise:N1}", textAlign);

		DrawStatuses();
		DrawApplyStatus();
		DrawDamage();
	}

	void SwitchTarget(int delta)
	{
		_targetIndex = (_targetIndex + delta) % _characters.Length;
		if (_targetIndex < 0)
			_targetIndex = Math.Max(_characters.Length - 1, 0);

		if (_targetIndex < _characters.Length)
			target = _characters[_targetIndex];
	}

	private void DrawDamage()
	{
		_damageUnfolded = Fold.DoGUIHeader(_damageUnfolded, "Apply Damage");
		if (!_damageUnfolded) return;
		using (new RGUI.IndentScope())
		{
			_damageInfo = RGUI.Field(_damageInfo, "Damage");

			if (GUILayout.Button("Apply Damage"))
				target.TakeDamage(_damageInfo);
		}
	}

	private void DrawStatuses()
	{
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
				foreach (var gauge in target.GetGauges())
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

	private void DrawApplyStatus()
	{
		_statusInfoUnfolded = Fold.DoGUIHeader(_statusInfoUnfolded, "Apply Status");
		if (!_statusInfoUnfolded) return;

		using (new RGUI.IndentScope())
		{
			_statusInfo.status = RGUI.Field(_statusInfo.status, "Status");
			_statusInfo.amount = RGUI.Slider(_statusInfo.amount, 0f, 1f, "Amount");
			_statusInfo.damage = RGUI.Field(_statusInfo.damage, "Damage");
			_statusInfo.strength = RGUI.Field(_statusInfo.strength, "Strength");

			using (new GUILayout.HorizontalScope())
			{
				if (GUILayout.Button("Add Status"))
					target.ApplyStatus(_statusInfo);
				if (GUILayout.Button("Clear Status"))
					target.ClearStatus(_statusInfo.status);
			}
		}
	}

	protected override string title => "Debug Status Memi";
}