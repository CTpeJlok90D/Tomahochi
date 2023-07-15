using DialogSystem;
using Pets;
using UnityEngine;
using UnityEngine.UI;

public class PrayQuest : Quest
{
	[SerializeField] private Dialog _dialog;
	[SerializeField] private Dialoger _dialoger;
	[SerializeField] private GameObject _prayWindow;
	[SerializeField] private Button _prayButton;
	[SerializeField] private Button _backButton;
	[SerializeField] private GameObject[] _rooms;
	[SerializeField] private Pet _startPet;
	[SerializeField] private LootView _viewPrefab;
	[SerializeField] private Transform _content;
	[SerializeField] private Prayer _prayer;
	[SerializeField] private GameObject _prayHint;
	[SerializeField] private GameObject _exitHint;
	public override void OnQuestBegin()
	{
		base.OnQuestBegin();
		_dialoger.StartDialog(_dialog);
		foreach (GameObject room in _rooms)
		{
			room.SetActive(true);
		}
		_dialog.StoryEnded.AddListener(OnStoryEnded);
		_prayButton.onClick.AddListener(OnClick);
		_backButton.onClick.AddListener(OnExit);
		_backButton.interactable = false;
	}

	private void OnStoryEnded()
	{
		_prayWindow.SetActive(true);
		_prayHint.SetActive(true);
	}

	private void OnClick()
	{
		_prayButton.onClick.RemoveListener(OnClick);
		_prayer.PrayX(0);
		_startPet.ApplyLoot();
		_prayer.ShowLoot(_startPet);
		_backButton.interactable = true;
		_prayButton.interactable = false;
		_prayHint.SetActive(false);
		_exitHint.SetActive(true);
	}

	private void OnExit()
	{
		Complete();
		_exitHint.SetActive(false);
	}
}