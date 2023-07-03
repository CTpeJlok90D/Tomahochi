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
	[SerializeField] private Image _background;

	private string _moraPriceFormat;
	private string _gemsPriceFormat;
	public delegate void SelectItem(SoldItem item);
	private SelectItem _function;

	public Image Background => _background;

	public void OnPointerClick(PointerEventData eventData)
	{
		_function.Invoke(_soldItem);
	}

	public SoldItemView Init(SoldItem item, SelectItem function)
	{
		_function = function;
		_soldItem = item;
		_moraPriceFormat = _moraPriceCaption.text;
		_gemsPriceFormat = _gemsPriceCaption.text;
		_image.sprite = _soldItem.Item.ViewSprite;

		_moraPriceCaption.text = string.Format(_moraPriceFormat, _soldItem.MoraPrice);
		_moraPriceCaption.gameObject.SetActive(_soldItem.MoraPrice != 0);

		_gemsPriceCaption.text = string.Format(_gemsPriceFormat, _soldItem.GemsPrice);
		_gemsPriceCaption.gameObject.SetActive(_soldItem.GemsPrice != 0);
		return this;
	}
}
