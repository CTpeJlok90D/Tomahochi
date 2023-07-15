using DialogSystem;
using UnityEngine;
using UnityEngine.UI;

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
	[Header("Hints")]
	[SerializeField] private GameObject _campfireClickHint;
	[SerializeField] private GameObject _selectRecipeHint;
	[SerializeField] private GameObject _startCookHint;

	public override void OnQuestBegin()
	{
		base.OnQuestBegin();
		_cookPlace.SetActive(true);
		_dialoger.StartDialog(_introDialog);
		_introDialog.StoryEnded.AddListener(OnStoryEnd);
		_campfireSelecteble.Selected += OnCampfireSelect;
		_cookMiniGame.GameStarted += OnCookMiniGameStart;
		_cookMiniGame.GameEnded += OnCookMiniGameEnd;
		_ingridientForCook.AddOnStorage(1);
	}

	private void OnStoryEnd()
	{
		_introDialog.StoryEnded.RemoveListener(OnStoryEnd);

		_campfireClickHint.SetActive(true);
	}

	public void OnCampfireSelect(Selecteble selected)
	{
		if (selected.gameObject != _campfire.gameObject)
		{
			_movePanel.enabled = true;
			return;
		}
		_campfireSelecteble.Selected -= OnCampfireSelect;

		_campfire.SelectedRecipeChanged += OnCampfireSelectedRecipe;
		_dialoger.StartDialog(_cookTutorialDialog);
		_movePanel.enabled = false;
		_campfireClickHint.SetActive(false);
		_selectRecipeHint.SetActive(true);
	}

	private void OnCampfireSelectedRecipe(Recipe recipe)
	{
		_campfire.SelectedRecipeChanged -= OnCampfireSelectedRecipe;

		_selectRecipeHint.SetActive(false);
		_startCookHint.SetActive(true);
	}

	private void OnCookMiniGameStart()
	{
		_cookMiniGame.GameStarted -= OnCookMiniGameStart;

		_cookMiniGame.TimeScale = 0;
		_dialoger.StartDialog(_cookDialogHint);
		_cookDialogHint.StoryEnded.AddListener(OnCookDialogEnd);
		_startCookHint.SetActive(false);
	}

	private void OnCookDialogEnd()
	{
		_cookMiniGame.TimeScale = 0.7f;
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
}
