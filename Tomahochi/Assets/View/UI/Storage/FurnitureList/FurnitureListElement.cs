using Saving;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FurnitureListElement : MonoBehaviour, IPointerDownHandler
{
	[SerializeField] private FurnitureInfo _info;
	[SerializeField] private Image _imageView;
	[SerializeField] private TMP_Text _nameCaption;
	[SerializeField] private TMP_Text _countCaption;
	[SerializeField] private BuildPreview _buildPreviewPrefab;

	private string _nameFormat;
	private string _countFrormat;

	public FurnitureListElement Init(FurnitureInfo info)
	{
		_nameFormat = _nameCaption.text;
		_countFrormat = _countCaption.text;

		_info = info;
		_nameCaption.text = string.Format(_nameFormat, info.name);
		UpdateCount();
		_imageView.sprite = _info.ViewSprite;

		return this;
	}

	private void OnEnable()
	{
		PlayerDataContainer.FurnitureOnStarageCountChanged.AddListener(OnFurnitureCountChange);
	}

	private void OnDisable()
	{
		PlayerDataContainer.FurnitureOnStarageCountChanged.AddListener(OnFurnitureCountChange);
	}

	public void UpdateCount()
	{
		_countCaption.text = string.Format(_countFrormat, PlayerDataContainer.GetFurnitureCountOnStorage(_info.name));
	}

	private void OnFurnitureCountChange(Storageble info , int count)
	{
		if (info.name != _info.name)
		{
			return;
		}
		UpdateCount();
		if (count == 0)
		{
			Destroy(gameObject);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		StartCoroutine(PlaceCoroutine());
	}

	private IEnumerator PlaceCoroutine()
	{
		Furniture furniture = new()
		{
			SystemName = _info.name
		};
		BuildPreview buildPreview = Instantiate(_buildPreviewPrefab).Init(_info.ViewSprite, _info.BuildCheckPoints);

		while (buildPreview.Result == BuildPreview.InstallResult.InProgress)
		{
			yield return null;
		}
		if (buildPreview.Result == BuildPreview.InstallResult.Error)
		{
			Destroy(buildPreview.gameObject);
			yield break;
		}
		furniture.Position = buildPreview.transform.position;
		Destroy(buildPreview.gameObject);

		FurnitureView.CreatyByFurniture(furniture);
	}
}
