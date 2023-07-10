using TMPro;
using UnityEngine;
public class CurrencyDriverView : MonoBehaviour
{
	[SerializeField] private FurnitureCurrencyDriver _Driver;
	[SerializeField] private TMP_Text _moraCountCaption;
	[SerializeField] private TMP_Text _gemsCountCaption;
	[SerializeField] private SpriteRenderer[] _viewSprites;
	[SerializeField] private MeshRenderer[] _viewMeshs;

	private string _moraCountFormat;
	private string _gemsCountFormat;

	private void Start()
	{
		if (_moraCountCaption != null)
		{
			_moraCountFormat = _moraCountCaption.text;
		}
		if (_gemsCountCaption != null)
		{
			_gemsCountFormat = _gemsCountCaption.text;
		}
	}

	private void Update()
	{
		if (_moraCountCaption != null)
		{
			_moraCountCaption.text = string.Format(_moraCountFormat, _Driver.MoraCount);
		}
		if (_gemsCountCaption != null)
		{
			_gemsCountCaption.text = string.Format(_gemsCountFormat, _Driver.GemsCount);
		}
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