using Saving;
using TMPro;
using UnityEngine;

public class CurrencyView : MonoBehaviour
{
	[SerializeField] private TMP_Text _text;
	[SerializeField] private Currency _currency;
	private string _textFormat;

	private void Start()
	{
		_textFormat = _text.text;
		switch (_currency)
		{
			case Currency.Mora:
				_text.text = string.Format(_textFormat, PlayerDataContainer.MoraCount);
				break;
			case Currency.Gems:
				_text.text = string.Format(_textFormat, PlayerDataContainer.GemsCount);
				break;
		}
		
	}

	private void OnEnable()
	{
		switch (_currency)
		{
			case Currency.Mora:
				PlayerDataContainer.MoraCountChanged.AddListener(OnValueChange);
				break;
			case Currency.Gems:
				PlayerDataContainer.GemsCountChanged.AddListener(OnValueChange);
				break;
		}
	}

	private void OnValueChange(int value)
	{
		_text.text = string.Format(_textFormat, value);
	}

	private enum Currency
	{
		Mora,
		Gems
	}
}
