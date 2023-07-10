using Saving;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureActionsPanel : MonoBehaviour
{
	[SerializeField] private GameObject _panel;
	[SerializeField] private Vector2 _panelOffect;
	[SerializeField] private Button _moveOnStorageButton;
	[SerializeField] private Button _moveFurnitureButton;
	[SerializeField] private BuildPreview _buildPreviewPrefab;

	private FurnitureView _selectedFurnitureView;

	private void OnEnable()
	{
		Selecteble.SelectObjectChange += OnSelectObject;
		_moveOnStorageButton.onClick.AddListener(MoveOnStorage);
		_moveFurnitureButton.onClick.AddListener(MoveFurnuture);
		UI.ModeChanged += OnUIModeChange;
	}

	private void OnDisable()
	{
		Selecteble.SelectObjectChange -= OnSelectObject;
		_moveOnStorageButton.onClick.AddListener(MoveOnStorage);
		_moveFurnitureButton.onClick.AddListener(MoveFurnuture);
		UI.ModeChanged -= OnUIModeChange;
	}

	private void OnUIModeChange(UIMode mode)
	{
		if (mode != UIMode.EditRoomMode)
		{
			gameObject.SetActive(false);
		}
	}

	private void MoveOnStorage()
	{
		_selectedFurnitureView?.MoveOnStorage();
		_selectedFurnitureView = null;
	}

	private void MoveFurnuture()
	{
		if (_selectedFurnitureView == null)
		{
			return;
		}
		_selectedFurnitureView.MoveOnStorage();
		StartCoroutine(PlaceCoroutine(_selectedFurnitureView.Info));
		_selectedFurnitureView = null;
	}

	private IEnumerator PlaceCoroutine(FurnitureInfo info)
	{
		Furniture furniture = new()
		{
			SystemName = info.name
		};
		BuildPreview buildPreview = Instantiate(_buildPreviewPrefab).Init(info.ViewSprite, info.BuildCheckPoints);

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

		PlayerDataContainer.PlaceFurniture(furniture);
		FurnitureView furnitureView = FurnitureView.CreatyByFurniture(furniture);
	}

	private void OnSelectObject(Selecteble selected)
	{
		if (selected != null && selected.TryGetComponent(out FurnitureView view))
		{
			_panel.transform.position = (Vector2)Input.mousePosition + _panelOffect;
			_selectedFurnitureView = view;
			_panel.SetActive(true);
			return;
		}
		_panel.SetActive(false);
	}
}
