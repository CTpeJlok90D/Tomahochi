using Pets;
using Saving;
using System.Linq;
using UnityEngine;

public class SleepSpotView : MonoBehaviour
{
	[SerializeField] private FurnitureView _furnitureView;
	[SerializeField] private Selecteble _select;
	[SerializeField] private PetSaveInfo _sleepingPet;
	[SerializeField] private Transform _sleepTransform;

	public PetSaveInfo SleepingPet => _sleepingPet;

	public Transform SleepTransform => _sleepTransform;

	private void OnEnable()
	{
		_select.Selected += OnSelect;
		_select.Deselected += OnDeselect;
	}

	private void OnDisable()
	{
		_select.Selected -= OnSelect;
		_select.Deselected -= OnDeselect;
	}

	private void Update()
	{
		if (_sleepingPet != null && _sleepingPet.SleepingBedID == string.Empty)
		{
			_sleepingPet = null;
		}
	}

	private void OnSelect(Selecteble selecteble) => OnSelect();
	private void OnSelect()
	{
		UI.PetList.PetClicked += OnPetChoose;
		UI.PetList.gameObject.SetActive(false);
		UI.PetList.Show(PlayerDataContainer.UnlockedPets.Where((pet) => pet.CanSleep()).ToArray());
	}

	private void OnDeselect()
	{
		UI.PetList.PetClicked -= OnPetChoose;
		UI.PetList?.gameObject.SetActive(false);
	}

	private void OnPetChoose(PetSaveInfo info)
	{
		PutPet(info);
	}
	public void PutPet(PetSaveInfo info)
	{
		if (PutToBed(info))
		{
			PetView view = PetView.GetPetViewByInfo(info);
			view.LaySleep(this);
			UI.PetList.Show(PlayerDataContainer.UnlockedPets.Where((pet) => pet.CanSleep()).ToArray());
		}
	}
	private bool PutToBed(PetSaveInfo info)
	{
		if (_sleepingPet != null || info.CanSleep() == false)
		{
			return false;
		}
		_sleepingPet = info;
		info.LaySleep(_furnitureView.Source);
		return true;
	}
}
