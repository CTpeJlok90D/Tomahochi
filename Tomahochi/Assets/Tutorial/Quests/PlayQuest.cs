using DialogSystem;
using Memory;
using Saving;
using UnityEngine;
using UnityEngine.UI;

public class PlayQuest : Quest
{
	[SerializeField] private GameObject _playMiniGamesPanel;
	[SerializeField] private Button _playMiniGamesButton;
	[SerializeField] private Dialoger _dialoger;
	[SerializeField] private Dialog _startDialog;
	[SerializeField] private Dialog _chooseGameDialog;

	public override void OnQuestBegin()
	{
		base.OnQuestBegin();
		_dialoger.StartDialog(_startDialog);
		_playMiniGamesPanel.SetActive(true);
		_playMiniGamesButton.onClick.AddListener(OnButtonClick);
		PlayerDataContainer.SavePlayerData();
	}

	private void OnButtonClick()
	{
		_dialoger.StartDialog(_chooseGameDialog);
		_playMiniGamesButton.onClick.RemoveListener(OnButtonClick);
		new Save(Tutorial.SAVE_KEY, new Tutorial.Progress(1));
	}
}
