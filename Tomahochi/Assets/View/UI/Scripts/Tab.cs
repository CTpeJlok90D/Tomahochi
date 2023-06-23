using UnityEngine;

public class Tab : MonoBehaviour
{
	public void Enable()
	{
		foreach (Transform transform in transform.parent)
		{
			transform.gameObject.SetActive(false);
		}
		gameObject.SetActive(true);

	}
}
