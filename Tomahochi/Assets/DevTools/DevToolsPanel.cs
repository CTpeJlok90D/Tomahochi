using Pets;
using Saving;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DevToolsPanel : MonoBehaviour
{
	[Header("Pet")]
	[SerializeField] private TMP_Dropdown _petsDropdown;
	[Space(5)]
	[SerializeField] private Button _setHungerButton;
	[SerializeField] private TMP_InputField _hungerField;
	[Space(5)]
	[SerializeField] private Button _setDrinkButton;
	[SerializeField] private TMP_InputField _drinkField;
	[Space(5)]
	[SerializeField] private Button _setEnergyButton;
	[SerializeField] private TMP_InputField _energyField;
	[Space(5)]
	[SerializeField] private Button _setJoyButton;
	[SerializeField] private TMP_InputField _joyField;
	[Space(5)]
	[SerializeField] private Button _addXpButton;
	[SerializeField] private TMP_InputField _xpField;
	[Header("Mora and gems")]
	[SerializeField] private Button _setMoraButton;
	[SerializeField] private TMP_InputField _moraInputField;
	[Space(5)]
	[SerializeField] private Button _setGemsButton;
	[SerializeField] private TMP_InputField _gemsInputField;
	[Header("Add pet")]
	[SerializeField] private TMP_Dropdown _allPetDropdown;
	[SerializeField] private Button _addButton;
	[Header("Storageble")]
	[SerializeField] private TMP_Dropdown _storagebleDropdown;
	[SerializeField] private TMP_InputField _storageField;
	[SerializeField] private Button _addOnStorageButton;

	private Pet[] _allPets;
	private PetSaveInfo[] _petSaveInfos;
	private Storageble[] _storagebles;

	private void Awake()
	{
		_petsDropdown.ClearOptions();
		List<TMP_Dropdown.OptionData> petsData = new();
		_petSaveInfos = PlayerDataContainer.UnlockedPets;
		foreach (PetSaveInfo info in _petSaveInfos) 
		{
			petsData.Add(new(info.Pet.name));
		}
		_petsDropdown.AddOptions(petsData);


		_storagebleDropdown.ClearOptions();
		_storagebles = Resources.LoadAll<Storageble>("");
		List<TMP_Dropdown.OptionData> storageblesData = new();
		foreach (Storageble storageble in _storagebles)
		{
			storageblesData.Add(new(storageble.name));
		}
		_storagebleDropdown.AddOptions(storageblesData);

		_allPetDropdown.ClearOptions();
		_allPets = Resources.LoadAll<Pet>("");
		List<TMP_Dropdown.OptionData> allPetData = new();
		foreach (Pet pet in _allPets)
		{
			allPetData.Add(new(pet.name));
		}
		_allPetDropdown.AddOptions(allPetData);
	}

	private void OnEnable()
	{
		_setHungerButton.onClick.AddListener(SetHunger);
		_setDrinkButton.onClick.AddListener(SetWater);
		_setEnergyButton.onClick.AddListener(SetEnegry);
		_setJoyButton.onClick.AddListener(SetJoy);
		_addXpButton.onClick.AddListener(GiveXp);

		_addOnStorageButton.onClick.AddListener(AddOnStorage);

		_setMoraButton.onClick.AddListener(SetMora);
		_setGemsButton.onClick.AddListener(SetGems);

		_addButton.onClick.AddListener(AddPet);
	}

	private void OnDisable()
	{
		_setHungerButton.onClick.RemoveListener(SetHunger);
		_setDrinkButton.onClick.RemoveListener(SetWater);
		_setEnergyButton.onClick.RemoveListener(SetEnegry);
		_setJoyButton.onClick.RemoveListener(SetJoy);
		_addXpButton.onClick.RemoveListener(GiveXp);

		_addOnStorageButton.onClick.RemoveListener(AddOnStorage);

		_setMoraButton.onClick.RemoveListener(SetMora);
		_setGemsButton.onClick.RemoveListener(SetGems);

		_addButton.onClick.RemoveListener(AddPet);
	}

	private void SetHunger()
	{
		_petSaveInfos[_petsDropdown.value].Food = float.Parse(_hungerField.text);
	}

	private void SetWater()
	{
		_petSaveInfos[_petsDropdown.value].Water = float.Parse(_drinkField.text);
	}

	private void SetEnegry()
	{
		_petSaveInfos[_petsDropdown.value].Energy = float.Parse(_energyField.text);
	}

	private void SetJoy()
	{
		_petSaveInfos[_petsDropdown.value].Joy = float.Parse(_joyField.text);
	}

	private void GiveXp()
	{
		_petSaveInfos[_petsDropdown.value].GainXP(float.Parse(_xpField.text));
	}

	private void AddOnStorage()
	{
		_storagebles[_storagebleDropdown.value].AddOnStorage(int.Parse(_storageField.text));
	}

	private void SetMora()
	{
		PlayerDataContainer.MoraCount = int.Parse(_moraInputField.text);
	}

	private void SetGems()
	{
		PlayerDataContainer.GemsCount = int.Parse(_gemsInputField.text);
	}

	private void AddPet()
	{
		PlayerDataContainer.AddPet(new PetSaveInfo(_allPets[_allPetDropdown.value]));
	}
}