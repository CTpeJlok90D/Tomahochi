using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoldItemList : MonoBehaviour
{
	[SerializeField] private SoldItemView _prefab;
	[SerializeField] private Transform _content;
	[SerializeField] private List<SoldItem> _list;
	[SerializeField] private TMP_Text _nameCaption;
	[SerializeField] private TMP_Text _descriptionCaption;
	[SerializeField] private Button _buyItem;

	[SerializeField] private SoldItem _selectedItem;
	[SerializeField] private string _nameFormat;
	[SerializeField] private string _descriptionFormat;

	private Dictionary<SoldItem, SoldItemView> _itemsViews = new();

	private void Start()
	{
		_nameFormat = _nameCaption.text;
		_descriptionFormat = _descriptionCaption.text;

		LoadItems();
	}

	private void OnEnable()
	{
		_buyItem.onClick.AddListener(OnBuyClick);
	}

	private void OnDisable()
	{
		_buyItem.onClick.RemoveListener(OnBuyClick);
	}

	private void LoadItems()
	{
		Clear();
		foreach (SoldItem item in _list)
		{
			SoldItemView view = Instantiate(_prefab, _content).Init(item, SelectItem);
			_itemsViews.Add(item, view);
		}
		SelectItem(_list[0]);
	}

	private void SelectItem(SoldItem item)
	{
		if (_selectedItem != null)
		{
			_itemsViews[_selectedItem].Background.enabled = false;
		}
		_itemsViews[item].Background.enabled = true;
		_selectedItem = item;
		UpdateCaption();
	}

	private void UpdateCaption()
	{
		_nameCaption.text = string.Format(_nameFormat, _selectedItem.Name, _selectedItem.Count, _selectedItem.Item.GetStorageCount());
		_descriptionCaption.text = string.Format(_descriptionFormat, _selectedItem.Description);
	}

	public void OnBuyClick()
	{
		_selectedItem.Buy();
		UpdateCaption();
	}

	private void Clear()
	{
		_itemsViews.Clear();
		foreach (Transform childs in _content.transform)
		{
			Destroy(childs.gameObject);
		}
	}
}