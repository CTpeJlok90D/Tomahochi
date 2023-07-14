using DialogSystem;
using UnityEngine;

public class FinishDialogQuest : Quest
{
	[SerializeField] private Dialog _startDialog;
	[SerializeField] private Dialog _finishDialog;
	[SerializeField] private Dialoger _dialoger;
	public override void OnQuestBegin()
	{
		base.OnQuestBegin();
		_dialoger.StartDialog(_startDialog);
		_finishDialog.StoryEnded.AddListener(OnDialogFinish);
	}

	public void OnDialogFinish()
	{
		Complete();
		_finishDialog.StoryEnded.RemoveListener(OnDialogFinish);
	}
}
