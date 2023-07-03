using Pets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetUI : MonoBehaviour
{
	[SerializeField] private GameObject _parent;
	[SerializeField] private TMP_Text _gemsCaption;
	[SerializeField] private TMP_Text _moraCaption;
	[SerializeField] private TMP_Text _nameCaption;
	[SerializeField] private TMP_Text _levelCaption;
	[SerializeField] private TMP_Text _elevateCostCaption;
	[SerializeField] private Image _joyImage;
	[SerializeField] private Image _foodImage;
	[SerializeField] private Image _waterImage;
	[SerializeField] private Image _sleepImage;
	[SerializeField] private Slider _slider;
	[SerializeField] private Button _elevateButton;
	[SerializeField] private Image[] _starsImages;
	[SerializeField] private Image[] _backGroundStars;

	private string _gemsCaptionFormat;
	private string _moraCaptionFormat;
	private string _nameCaptionFormat;
	private string _levelCaptionFormat;
	private string _elevateCostFormat;

	private Pet _pet;
	private PetSaveInfo _petInfo;

	private void Start()
	{
		_gemsCaptionFormat = _gemsCaption.text;
		_moraCaptionFormat = _moraCaption.text;
		_nameCaptionFormat = _nameCaption.text;
		_levelCaptionFormat = _levelCaption.text;
		_elevateCostFormat = _elevateCostCaption.text;
	}

	private void OnEnable()
	{
		Selecteble.SelectObjectChange += OnSelectedObjectChange;
		_elevateButton.onClick.AddListener(ElevatePet);
	}

	private void OnDisable()
	{
		Selecteble.SelectObjectChange -= OnSelectedObjectChange;
		_elevateButton.onClick.RemoveListener(ElevatePet);
		OnSelectedObjectChange(null);
	}

	private void ElevatePet()
	{
		_petInfo.Elevate();
	}

	private void OnSelectedObjectChange(Selecteble selecteble)
	{
		if (selecteble != null && selecteble.gameObject.TryGetComponent(out PetView pet))
		{
			_pet = pet.Pet;
			_petInfo = pet.PetInfo;

			_nameCaption.text = string.Format(_nameCaptionFormat, _pet.ViewName);

			_parent.SetActive(true);
			return;
		}
		_parent.SetActive(false);
	}

	private void LateUpdate()
	{
		if (_pet == null)
		{
			return;
		}
		UpdateStats();
	}

	public void UpdateStats()
	{
		_levelCaption.text = string.Format(_levelCaptionFormat, _petInfo.CurrentLevel % _pet.EvelateEveryLevel, _pet.EvelateEveryLevel);
		_gemsCaption.text = string.Format(_gemsCaptionFormat, (int)_petInfo.GemsCount, _petInfo.GemsStorage);
		_moraCaption.text = string.Format(_moraCaptionFormat, (int)_petInfo.MoraCount, _petInfo.MoraStorage);
		_elevateCostCaption.text = string.Format(_elevateCostFormat, _petInfo.ElevateCost());

		_joyImage.fillAmount = _petInfo.Joy / 100;
		_foodImage.fillAmount = _petInfo.Food / 100;
		_waterImage.fillAmount = _petInfo.Water / 100;
		_sleepImage.fillAmount = _petInfo.Energy / 100;


		_slider.value = _petInfo.CurrentXP / _pet.XPToLevelUp;
		_elevateButton.gameObject.SetActive(_petInfo.NeedEvelate);
		int starNumber = 1;
		foreach (Image image in _backGroundStars)
		{
			image.gameObject.SetActive(starNumber <= _pet.MaxElevateCount);
			starNumber++;
		}
		starNumber = 1;
		foreach (Image image in _starsImages)
		{
			image.enabled = starNumber <= _petInfo.ElevateCount;
			starNumber++;
		}
	}
}
