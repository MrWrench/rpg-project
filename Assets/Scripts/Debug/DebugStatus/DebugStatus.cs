using System;
using System.Globalization;
using System.Linq;
using StatusFX;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UniRx;
using UnityEngine.Serialization;

namespace Debug.DebugStatus
{
	public class DebugStatus : MonoBehaviour
	{
		[SerializeField] private Character? target;
		[SerializeField] private Text? nameLabel;
		[FormerlySerializedAs("health")] [SerializeField] private Text? healthLabel;
		[SerializeField] private Dropdown? statusDropdown;
		[FormerlySerializedAs("amount")] [SerializeField] private Slider? amountSlider;
		[SerializeField] private Text? amountValueLabel;
		[FormerlySerializedAs("strength")] [SerializeField] private InputField? strengthInput;
		[FormerlySerializedAs("damage")] [SerializeField] private InputField? damageInput;
		[SerializeField] private Button? addStatusBtn;
		[SerializeField] private Button? clearStatusBtn;
		private EnumStatusType[] _statusTypes = null!;

		private void Awake()
		{
			_statusTypes = Enum.GetValues(typeof(EnumStatusType)).Cast<EnumStatusType>().ToArray();
		}

		private void Start()
		{
			statusDropdown!.ClearOptions();
			statusDropdown!.AddOptions(Enum.GetNames(typeof(EnumStatusType))
				.Select(x => new Dropdown.OptionData(x.ToLower())).ToList());

			amountSlider.OnValueChangedAsObservable().Subscribe(x => amountValueLabel!.text = x.ToString("P1"));
			addStatusBtn!.onClick.AddListener(() =>
			{
				var statusType = _statusTypes[statusDropdown.value];
				var amount = amountSlider!.value;
				var strength = float.Parse(strengthInput!.text, CultureInfo.InvariantCulture);
				var damage = float.Parse(damageInput!.text, CultureInfo.InvariantCulture);
				target.ApplyStatus(new AddStatusInfo(statusType, amount, damage, strength));
			});
			
			clearStatusBtn!.onClick.AddListener(() =>
			{
				var statusType = _statusTypes[statusDropdown.value];
				target.ClearStatus(statusType);
			}); 
		}

		private void Update()
		{
			healthLabel!.text = target!.health.ToString("N1");
		}
	}
}