using Pets;
using UnityEngine;

public class SleepSpotView : MonoBehaviour
{
	[SerializeField] private FurnitureView _furnitureView;
	[SerializeField] private Selecteble _select;
	[SerializeField] private string _sleepingPet;
	[SerializeField] private Transform _sleepTransform;

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

	private void OnSelect(Selecteble selecteble) => OnSelect();
	private void OnSelect()
	{
		UI.PetList.PetClicked += OnPetChoose;
		UI.PetList.Show();
	}

	private void OnDeselect()
	{
		UI.PetList.PetClicked -= OnPetChoose;
		UI.PetList?.Hide();
	}

	private void OnPetChoose(PetSaveInfo info)
	{
		if (PutToBed(info))
		{
			PetView view = PetView.GetPetViewByInfo(info);
			view.LaySleep(this);
		}
	}

	private bool PutToBed(PetSaveInfo info)
	{
		if (string.IsNullOrEmpty(_sleepingPet) == false || info.CanSleep() == false)
		{
			return false;
		}
		info.LaySleep(_furnitureView.Source);
		return true;
	}
}
