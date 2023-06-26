using Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpandHomePanel : MonoBehaviour
{
	[SerializeField] private Button _yesButton;
	[SerializeField] private Button _noButton;
	[SerializeField] private GameObject _panelObject;
	[SerializeField] private HomeView _homeView;
	[SerializeField] private TMP_Text _costCaption;

	private string _costFormat;
	private BuildPlace _selectedBuildPlace;
	private Selecteble _selecteble;

	private void Start()
	{
		_costFormat = _costCaption.text;
	}

	private void OnEnable()
	{
		Selecteble.SelectObjectChange += OnSelectedObjectChanged;
		_yesButton.onClick.AddListener(OnYesClick);
		_noButton.onClick.AddListener(OnNoClick);
	}

	private void OnDisable()
	{
		Selecteble.SelectObjectChange -= OnSelectedObjectChanged;
		_yesButton.onClick.RemoveListener(OnYesClick);
		_noButton.onClick.AddListener(OnNoClick);
	}

	private void OnSelectedObjectChanged(Selecteble selectedObject)
	{
		if (selectedObject != null && selectedObject.TryGetComponent(out BuildPlace place))
		{
			_selectedBuildPlace = place;
			_selecteble = selectedObject;
			_panelObject.SetActive(true);
			_yesButton.interactable = PlayerDataContainer.MoraCount >= PlayerDataContainer.BuildCost;
			_costCaption.text = string.Format(_costFormat, PlayerDataContainer.BuildCost);
			return;
		}
		_selectedBuildPlace = null;
		_selecteble = null;
		_panelObject.SetActive(false);
	}

	private void OnYesClick()
	{
		_homeView.AddRoom(_selectedBuildPlace.PositionOnGrid);
		Destroy(_selectedBuildPlace.gameObject);
	}

	private void OnNoClick()
	{
		_selecteble?.Deselect();
	}
}
