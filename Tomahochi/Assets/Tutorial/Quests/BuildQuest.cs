using DialogSystem;
using Saving;
using UnityEngine;
using UnityEngine.UI;

public class BuildQuest : Quest
{
	[SerializeField] private GameObject _furnitureListUI;
	[SerializeField] private Button _editModeButton;
	[SerializeField] private Storageble _bed;
	[SerializeField] private GameObject _enterEditModeHint;
	[SerializeField] private GameObject _buildHint;

	[SerializeField] private Dialoger _dialoger;
	[SerializeField] private Dialog _startDialog;
	
	public override void OnQuestBegin()
	{
		base.OnQuestBegin();
		_dialoger.StartDialog(_startDialog);
		_editModeButton.gameObject.SetActive(true);
		_startDialog.StoryEnded.AddListener(OnStartDialogEnd);
		_editModeButton.onClick.AddListener(OnEditModeEnther);
	}

	private void OnStartDialogEnd()
	{
		_startDialog.StoryEnded.RemoveListener(OnStartDialogEnd);

		_enterEditModeHint.SetActive(true);
	}

	private void OnEditModeEnther()
	{
		_editModeButton.onClick.RemoveListener(OnEditModeEnther);

		_enterEditModeHint.SetActive(false);
		_buildHint.SetActive(true);
		_bed.AddOnStorage(1);
		_bed.OnStorageCountChanged.AddListener(OnBedCountChagned);
		_furnitureListUI.SetActive(true);
		UI.Mode = UIMode.EditRoomMode;
		_editModeButton.interactable = false;
	}

	private void OnBedCountChagned(Storageble item, int count)
	{
		if (item.name != _bed.name)
		{
			return;
		}
		_bed.OnStorageCountChanged.RemoveListener(OnBedCountChagned);
		_editModeButton.onClick.AddListener(OnEditModeExit);
		_editModeButton.interactable = true;
		Selecteble.CanSelect = false;
		Complete();
	}

	private void OnEditModeExit()
	{
		UI.Mode = UIMode.Defualt;
		_editModeButton.interactable = false;
		_furnitureListUI.SetActive(false);
		Selecteble.CanSelect = true;
	}
}
