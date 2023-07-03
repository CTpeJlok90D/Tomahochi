using UnityEngine;
using UnityEngine.EventSystems;
public class DeselectButton : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		Selecteble.SelectedObject.Deselect();
	}
}
