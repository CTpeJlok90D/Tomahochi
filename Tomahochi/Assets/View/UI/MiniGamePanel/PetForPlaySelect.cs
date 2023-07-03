using Pets;
using Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetForPlaySelect : MonoBehaviour
{
	[SerializeField] private Image _petIcon;
	[SerializeField] private TMP_Text _petNameCaption;
	[SerializeField] private Button _nextButton;
	[SerializeField] private Button _previewButton;

	private string _petNameFormat;

	private int _selectedPetIndex;

	public PetSaveInfo SelectedPetInfo => PlayerDataContainer.UnlockedPets[SelectedPetIndex];
	public int SelectedPetIndex
	{
		get
		{
			return _selectedPetIndex;
		}
		set
		{
			_selectedPetIndex = Mathf.Clamp(value, 0, PlayerDataContainer.UnlockedPets.Length - 1);
			_petNameCaption.text = string.Format(_petNameFormat, SelectedPetInfo.Pet.ViewName);
			_petIcon.sprite = SelectedPetInfo.Pet.ViewSprite;
			PlayerDataContainer.PlayingPetIndex = _selectedPetIndex;
		}
	}

	private void Start()
	{
		_petNameFormat = _petNameCaption.text;
		SelectedPetIndex = 0;
	}

	private void OnEnable()
	{
		_nextButton.onClick.AddListener(NextPet);
		_previewButton.onClick.AddListener(PreviewPet);
	}

	private void OnDisable()
	{
		_nextButton.onClick.RemoveListener(NextPet);
		_previewButton.onClick.RemoveListener(PreviewPet);
	}


	public void NextPet()
	{
		SelectedPetIndex++;
	}

	public void PreviewPet()
	{
		SelectedPetIndex--;
	}
}