using Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Prayer : MonoBehaviour
{
	[SerializeField] private PrayDropTable _dropTable;
	[SerializeField] private LootView _lootViewPrefab;
	[SerializeField] private Transform _content;
	[SerializeField] private int _gemsCost = 180;
	[SerializeField] private GameObject _videoUI;
	[SerializeField] private VideoPlayer _videoPlayer;
	[SerializeField] private Button[] _prayButtons;
	[SerializeField] private TMP_Text _limitReachedCaption;

	private string _limitReachedFormat;

	private void Awake()
	{
		_limitReachedFormat = _limitReachedCaption.text;
	}

	private void OnEnable()
	{
		bool limitReached = PlayerDataContainer.UnlockedPets.Length >= PlayerDataContainer.PetCountPerRoom * PlayerDataContainer.GetRooms().Length;
		foreach (Button button in _prayButtons)
		{
			button.interactable = limitReached == false;
		}
		_limitReachedCaption.gameObject.SetActive(limitReached);
		_limitReachedCaption.text = string.Format(_limitReachedFormat, PlayerDataContainer.UnlockedPets.Length);
	}

	public void Pray()
	{
		PrayX(1);
	}

	private IEnumerator VideoCoruotine(int count)
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
		SpawnLoot(count);
	}

	private void OnDisable()
	{
		ClearLoot();
	}

	public void PrayX(int count)
	{
		if (PlayerDataContainer.GemsCount < _gemsCost * count)
		{
			return;
		}
		ClearLoot();

		StartCoroutine(VideoCoruotine(count));
	}

	public void SpawnLoot(int count)
	{
		PlayerDataContainer.GemsCount -= _gemsCost * count;
		List<ILootDrop> drops = new();
		for (int i = 0; i < count; i++)
		{
			ILootDrop drop = _dropTable.Pray();
			drops.Add(drop);
			drop.ApplyLoot();
			ShowLoot(drop);
		}
	}

	public void ShowLoot(ILootDrop loot)
	{
		Instantiate(_lootViewPrefab, _content).Init(loot);
	}
	public void ClearLoot()
	{
		if (_content == null)
		{
			return;
		}
		foreach (Transform child in _content)
		{
			Destroy(child.gameObject);
		}
	}
}
