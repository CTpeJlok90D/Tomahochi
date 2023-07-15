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
	[SerializeField] private GameObject _playButtonHint;

	public override void OnQuestBegin()
	{
		base.OnQuestBegin();
		_dialoger.StartDialog(_startDialog);
		_startDialog.StoryEnded.AddListener(OnDialogEnd);
		_playMiniGamesPanel.SetActive(true);
		_playMiniGamesButton.onClick.AddListener(OnButtonClick);
		PlayerDataContainer.SavePlayerData();
	}

	private void OnDialogEnd()
	{
		_startDialog.StoryEnded.RemoveListener(OnDialogEnd);

		_playButtonHint.SetActive(true);
	}

	private void OnButtonClick()
	{
		_playButtonHint.SetActive(false);
		_dialoger.StartDialog(_chooseGameDialog);
		_playMiniGamesButton.onClick.RemoveListener(OnButtonClick);
		new Save(Tutorial.SAVE_KEY, new Tutorial.Progress(1));
	}
}
