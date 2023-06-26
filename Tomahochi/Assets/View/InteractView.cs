using UnityEngine;
using UnityEngine.EventSystems;

public class InteractView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Vector3 _mouseOnSize = new Vector3(1.1f, 1.1f, 1.1f);
	private Vector3 _deualtSize;

	public void OnPointerEnter(PointerEventData eventData)
	{
		transform.localScale = _mouseOnSize;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		transform.localScale = _deualtSize;
	}

	private void Awake()
	{
		_deualtSize = transform.localScale;
	}

	private void OnMouseEnter()
	{
		transform.localScale = _mouseOnSize;
	}

	private void OnMouseExit()
	{
		transform.localScale = _deualtSize;
	}
}
