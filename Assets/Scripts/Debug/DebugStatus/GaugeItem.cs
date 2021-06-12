using StatusFX;
using UnityEngine;
using UnityEngine.UI;

namespace Debug.DebugStatus
{
	public class GaugeItem : MonoBehaviour
	{
		public BaseGaugeStatusFX? statusFX
		{
			get => _statusFX;
			set
			{
				_statusFX = value;
				if (_statusFX != null && nameLabel != null)
					nameLabel.text = _statusFX.statusType.ToString();
			}
		}

		private BaseGaugeStatusFX? _statusFX;
		[SerializeField] private Text? nameLabel;
		[SerializeField] private Text? amountLabel;

		private void Update()
		{
			if (statusFX != null)
			{
				amountLabel!.text = statusFX.amount.ToString("N1");
			}
		}
	}
}