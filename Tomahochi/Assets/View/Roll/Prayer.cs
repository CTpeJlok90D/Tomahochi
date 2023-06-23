using Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Prayer : MonoBehaviour
{
	[SerializeField] private PrayDropTable _dropTable;
	[SerializeField] private LootView _lootViewPrefab;
	[SerializeField] private Transform _content;
	[SerializeField] private int _gemsCost = 180;
	[SerializeField] private GameObject _videoUI;
	[SerializeField] private VideoPlayer _videoPlayer;
	public void Pray()
	{
		PrayX(1);
	}

	private IEnumerator VideoCoruotine()
	{
		_videoUI.SetActive(true);
		_videoPlayer.Play();
		double seconds = _videoPlayer.clip.length;
		while (seconds > 0)
		{
			seconds -= Time.deltaTime;
			yield return null;
		}
		_videoPlayer.Stop();
		_videoUI.SetActive(false);
	}

	public void PrayX(int count)
	{
		if (PlayerDataContainer.GemsCount < _gemsCost * count)
		{
			return;
		}

		StartCoroutine(VideoCoruotine());
		PlayerDataContainer.GemsCount -= _gemsCost * count;
		ClearLoot();
		List<ILootDrop> drops = new();
		for (int i = 0; i < count; i++)
		{
			drops.Add(_dropTable.Pray());
		}
		ShowLoot(drops);
	}
	public void ShowLoot(ILootDrop loot)
	{
		Instantiate(_lootViewPrefab, _content).Init(loot);
	}
	public void ShowLoot(List<ILootDrop> loots)
	{
		foreach (ILootDrop loot in loots)
		{
			Instantiate(_lootViewPrefab, _content).Init(loot);
		}
	}
	public void ClearLoot()
	{
		foreach (Transform child in _content)
		{
			Destroy(child.gameObject);
		}
	}
}
