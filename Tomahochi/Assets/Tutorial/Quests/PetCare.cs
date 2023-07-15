
using DialogSystem;
using Saving;
using UnityEngine;

public class PetCare : Quest
{
	[SerializeField] private Dialoger _dialoger;
	[SerializeField] private Dialog _dialog;
	[SerializeField] private Dialog _hintDialog;
	[SerializeField] private InWorldHint _hint;
	public override void OnQuestBegin()
	{
		base.OnQuestBegin();
		_dialoger.StartDialog(_dialog);
		Selecteble.SelectObjectChange += OnSelectedPet;
		_hint.Target = FindObjectOfType<PetView>().transform;
		_hint.gameObject.SetActive(true);
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
		_hint.gameObject.SetActive(false);
	}
}
