using DialogSystem;
using UnityEngine;

public class PetCare : Quest
{
	[SerializeField] private Dialoger _dialoger;
	[SerializeField] private Dialog _dialog;
	[SerializeField] private Dialog _hintDialog;
	public override void OnQuestBegin()
	{
		base.OnQuestBegin();
		_dialoger.StartDialog(_dialog);
		Selecteble.SelectObjectChange += OnSelectedPet;
	}

	private void OnSelectedPet(Selecteble selected)
	{
		if (selected == null)
		{
			Selecteble.SelectObjectChange -= OnSelectedPet;
			Complete();
			return;
		}
		_dialoger.StartDialog(_hintDialog);
	}
}
