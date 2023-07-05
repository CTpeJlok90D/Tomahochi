using Saving;
using UnityEngine;

public class MobileActiveOnly : MonoBehaviour
{
	private void Awake()
	{
		if (PlayerDataContainer.HaveInstance)
		{
			gameObject.SetActive(Init.Instance.mobile);
		}
	}
}
