using DialogSystem;
using UnityEngine;

public class CookQuest : Quest
{
	[SerializeField] private GameObject _cookPlace;
	[Header("Dialogs")]
	[SerializeField] private Dialog _introDialog;
	[SerializeField] private Dialog _cookTutorialDialog;
	[SerializeField] private Dialog _cookDialogHint;
	[SerializeField] private Dialog _failDialog;
	[SerializeField] private Dialog _succsessDialog;
	[SerializeField] private Dialoger _dialoger;
	[Space(10)]
	[SerializeField] private Selecteble _campfireSelecteble;
	[SerializeField] private Campfire _campfire;
	[SerializeField] private Storageble _ingridientForCook;
	[SerializeField] private MoveCameraPanel _movePanel;
	[SerializeField] private CookMiniGame _cookMiniGame;
	
	public override void OnQuestBegin()
	{
		base.OnQuestBegin();
		_cookPlace.SetActive(true);
		_dialoger.StartDialog(_introDialog);
		_campfireSelecteble.Selected += OnCampfireSelect;
		_cookMiniGame.GameStarted += OnCookMiniGameStart;
		_cookMiniGame.GameEnded += OnCookMiniGameEnd;
		_ingridientForCook.AddOnStorage(1);
	}

	private void OnCookMiniGameEnd(CookMiniGame.Result result)
	{
		if (result == CookMiniGame.Result.Success)
		{
			_cookMiniGame.GameEnded -= OnCookMiniGameEnd;
			_cookMiniGame.TimeScale = 1f;
			_dialoger.StartDialog(_succsessDialog);
			_succsessDialog.StoryEnded.AddListener(OnFinishFinalDialog);
			return;
		}
		_dialoger.StartDialog(_failDialog);
		_ingridientForCook.AddOnStorage(1);
	}

	private void OnFinishFinalDialog()
	{
		_succsessDialog.StoryEnded.RemoveListener(OnFinishFinalDialog);
		_movePanel.enabled = true;
		Complete();
	}

	private void OnCookMiniGameStart()
	{
		_cookMiniGame.TimeScale = 0;
		_dialoger.StartDialog(_cookDialogHint);
		_cookDialogHint.StoryEnded.AddListener(OnCookDialogEnd);
		_cookMiniGame.GameStarted -= OnCookMiniGameStart;
	}

	private void OnCookDialogEnd()
	{
		_cookMiniGame.TimeScale = 0.7f;
	}

	public void OnCampfireSelect(Selecteble selected)
	{
		if (selected.gameObject != _campfire.gameObject)
		{
			_movePanel.enabled = true;
			return;
		}
		_movePanel.enabled = false;
		_dialoger.StartDialog(_cookTutorialDialog);
		_campfireSelecteble.Selected -= OnCampfireSelect;
	}
}
