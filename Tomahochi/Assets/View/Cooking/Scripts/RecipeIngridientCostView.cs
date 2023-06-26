using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeIngridientCostView : MonoBehaviour
{
	[SerializeField] private Ingredient _linkedIngridient;
	[SerializeField] private int _count;
	[SerializeField] private Image _image;
	[SerializeField] private TMP_Text _countCaption;

	private string _countFormat;

	private void Start()
	{
		if (string.IsNullOrEmpty(_countFormat))
		{
			_countFormat = _countCaption.text;
		}
	}

	public Ingredient Ingredient
	{
		get
		{
			return _linkedIngridient;
		}
		set
		{
			_linkedIngridient = value;
			_image.sprite = _linkedIngridient.ViewSprite;
		}
	}
	public int Count
	{
		get
		{
			return _count;
		}
		set
		{
			if (string.IsNullOrEmpty(_countFormat))
			{
				_countFormat = _countCaption.text;
			}
			_count = value;
			_countCaption.text = string.Format(_countFormat, _count);
			gameObject.SetActive(_count != 0);
		}
	}
}