using TMPro;
using UnityEngine;

public class StoragebleFactureView : MonoBehaviour
{
	[SerializeField] private StoragebleFacture _facture;
	[SerializeField] private TMP_Text _countCaption;
	[SerializeField] private SpriteRenderer[] _viewSprites;
	[SerializeField] private MeshRenderer[] _viewMeshs;

	private string _countFormat;

	private void Start()
	{
		_countFormat = _countCaption.text;
	}

	private void Update()
	{
		_countCaption.text = string.Format(_countFormat, _facture.Ready, _facture.Max);
	}


	private void OnMouseEnter()
	{
		foreach (SpriteRenderer renderer in _viewSprites)
		{
			renderer.enabled = true;
		}
		foreach (MeshRenderer renderer in _viewMeshs)
		{
			renderer.enabled = true;
		}
	}

	private void OnMouseExit()
	{
		foreach (SpriteRenderer renderer in _viewSprites)
		{
			renderer.enabled = false;
		}
		foreach (MeshRenderer renderer in _viewMeshs)
		{
			renderer.enabled = false;
		}
	}
}
