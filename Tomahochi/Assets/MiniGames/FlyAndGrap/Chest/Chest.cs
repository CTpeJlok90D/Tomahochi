using UnityEngine;
using UnityExtentions;

public class Chest : MonoBehaviour
{
	[SerializeField] private Storageble[] _rewardList;
	[SerializeField] private Range _rewardItemsCount;
	[SerializeField] private Range _rewardCount;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out FlyAndGrapPlayer player))
		{
			int count = _rewardItemsCount;
			for (int i = 0; i < count; i++)
			{
				_rewardList[Random.Range(0, _rewardList.Length)].AddOnStorage(_rewardCount);
			}
		}
	}
}
