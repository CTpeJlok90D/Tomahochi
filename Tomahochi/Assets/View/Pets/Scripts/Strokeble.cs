using Pets;
using UnityEngine;

public class Strokeble : MonoBehaviour
{
	[SerializeField] private Selecteble _selecteble;
	[SerializeField] private PetView _pet;

	public PetView Pet => _pet;

	private void OnMouseDown()
	{
		if (_selecteble.IsSelected)
		{
			_pet.PetInfo.Stroke();
		}
	}
}
