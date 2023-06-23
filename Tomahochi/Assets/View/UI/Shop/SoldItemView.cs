using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoldItemView : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private SoldItem _soldItem;
	[SerializeField] private Image _image;
	[SerializeField] private TMP_Text _moraPriceCaption;
	[SerializeField] private TMP_Text _gemsPriceCaption;
	private string _moraPriceFormat;
	private string _gemsPriceFormat;
	private SelectItem _function;
	public void OnPointerClick(PointerEventData eventData)
	{
		_function.Invoke(_soldItem);
	}

	public delegate void SelectItem(SoldItem item);
	public SoldItemView Init(SoldItem item, SelectItem function)
	{
		_function = function;
		_soldItem = item;
		_moraPriceFormat = _moraPriceCaption.text;
		_gemsPriceFormat = _gemsPriceCaption.text;
		_image.sprite = _soldItem.Item.ViewSprite;

		_moraPriceCaption.text = string.Format(_moraPriceFormat, _soldItem.MoraPrice);
		if (_soldItem.MoraPrice == 0)
		{
			Destroy(_moraPriceCaption.gameObject);
		}
		_gemsPriceCaption.text = string.Format(_gemsPriceFormat, _soldItem.GemsPrice);
		if (_soldItem.GemsPrice == 0)
		{
			Destroy(_gemsPriceCaption.gameObject);
		}
		return this;
	}
}
