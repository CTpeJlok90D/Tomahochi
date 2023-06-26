using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
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
}
