using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{
	[SerializeField] private GraphicRaycaster _raycaster;
	private void Update()
	{
		if (Input.GetMouseButtonUp(0) == false)
		{
			return;
		}

		PointerEventData pointEvent = new(EventSystem.current);
		pointEvent.position = Input.mousePosition;
		List<RaycastResult> results = new();
		_raycaster.Raycast(pointEvent, results);
		if (results.Count > 1)
		{
			return;
		}

		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity);
		if (hit == false)
		{
			return;
		}
		Component[] components = hit.collider.gameObject.GetComponents(typeof(Selecteble));
		foreach (Component component in components)
		{
			(component as Selecteble).OnPlayerClick();
		}
	}
}
