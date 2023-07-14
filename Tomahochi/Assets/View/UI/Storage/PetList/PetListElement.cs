using Pets;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PetListElement : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private Image _headPlace;
	[SerializeField] private TMP_Text _nameCaption;

	private string nameFormat;

	public delegate void PetClickHandler(PetSaveInfo info);
	private event PetClickHandler _onPetClick;

	private PetSaveInfo _info;

	public event PetClickHandler OnPetClick
	{
		add
		{
			_onPetClick += value;
		}
		remove
		{
			_onPetClick -= value;
		}
	}

	public PetListElement Init(PetSaveInfo info)
	{
		Pet pet = info.Pet;
		nameFormat = _nameCaption.text;

		_info = info;
		_nameCaption.text = string.Format(nameFormat, pet.name);
		_headPlace.sprite = pet.ViewSprite;
		return this;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		_onPetClick?.Invoke(_info);
	}
}
