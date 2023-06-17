using Cinemachine;
using Pets;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SelectPetCollision : MonoBehaviour
{
	[SerializeField] private Canvas _selectedPetcanvas;
	[SerializeField] private CinemachineVirtualCamera _camera;
	[SerializeField] private int _cameraPryorityChange = 50;
	[SerializeField] private PetView _petView;

	private static SelectPetCollision _selectedPet;
	public static SelectPetCollision SelectedPet => _selectedPet;
	public PetView PetView => _petView;
	public bool Selected
	{
		get
		{
			return _selectedPet == this;
		}
		set
		{
			if (value)
			{
				Select();
			}
			else
			{
				Deselect();
			}
		}
	}

	private void OnMouseDown()
	{
		if (Selected)
		{
			_petView.PetInfo.Stroke();
		}
		Select();
	}

	public void Select()
	{
		if (Selected)
		{
			return;
		}
		_selectedPet?.Deselect();
		_selectedPet = this;
		_selectedPetcanvas.gameObject.SetActive(true);
		_camera.Priority += _cameraPryorityChange;
	}

	public void Deselect()
	{
		if (Selected == false)
		{
			return;
		}
		_selectedPet = null;
		_selectedPetcanvas.gameObject.SetActive(false);
		_camera.Priority -= _cameraPryorityChange;
	}

#if UNITY_EDITOR
	[SerializeField] private bool _selected = false;

	private void OnValidate()
	{
		if (Application.IsPlaying(gameObject))
		{
			Selected = _selected;
		}
	}
#endif
}
