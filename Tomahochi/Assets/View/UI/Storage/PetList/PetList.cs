using Pets;
using Saving;
using UnityEngine;

public class PetList : MonoBehaviour
{
	[SerializeField] private PetListElement _petListElementPrefab;
	[SerializeField] private Transform _content;

	public delegate void PetClickHandler(PetSaveInfo info);
	private event PetClickHandler _petClicked;
	public event PetClickHandler PetClicked
	{
		add
		{
			_petClicked += value;
		}
		remove
		{
			_petClicked -= value;
		}
	}

	public void Show(PetSaveInfo[] list)
	{
		ShowPets(list);
		gameObject.SetActive(true);
	}

	public void Show()
	{
		ShowPets(PlayerDataContainer.UnlockedPets);
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		Clear();
		gameObject.SetActive(false);
	}

	private void ShowPets(PetSaveInfo[] petList)
	{
		Clear();
		foreach (PetSaveInfo petInfo in petList)
		{
		 	PetListElement element = Instantiate(_petListElementPrefab, _content).Init(petInfo);
			element.OnPetClick += OnPetClick;
		}
	}

	private void OnPetClick(PetSaveInfo info)
	{
		_petClicked?.Invoke(info);
	}

	private void Clear()
	{
		foreach (Transform child in _content.transform)
		{
			Destroy(child.gameObject);
		}
	}
}
