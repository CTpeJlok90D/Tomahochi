using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity);
			if (hit && hit.collider.gameObject.TryGetComponent(out Selecteble selecteble))
			{
				selecteble.OnPlayerClick();
			}
		}
	}
}
