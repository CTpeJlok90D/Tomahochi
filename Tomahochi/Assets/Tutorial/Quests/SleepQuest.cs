using DialogSystem;
using Pets;
using Saving;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SleepQuest : Quest
{
	[SerializeField] private Dialoger _dialoger;
	[SerializeField] private Dialog _startDialog;
	[SerializeField] private Dialog _layPetSleep;
	[SerializeField] private Dialog _sleepDialog;
	[SerializeField] private Dialog _onBuildExit;
	[SerializeField] private PetList _petList;
	[SerializeField] private Button _exitBuildButton;

	private SleepSpotView _spot;

	public override void OnQuestBegin()
	{
		base.OnQuestBegin();
		_dialoger.StartDialog(_startDialog);
		Selecteble.SelectObjectChange += OnSelectObject;
		_exitBuildButton.onClick.AddListener(OnExitBuild);
	}

	private void OnExitBuild()
	{
		_exitBuildButton.onClick.RemoveListener(OnExitBuild);

		_dialoger.StartDialog(_onBuildExit);
	}

	private void OnSelectObject(Selecteble selected)
	{
		if (selected.TryGetComponent(out _spot) == false)
		{
			return;
		}
		Selecteble.SelectObjectChange -= OnSelectObject;

		_dialoger.StartDialog(_layPetSleep);
		_petList.Show(PlayerDataContainer.UnlockedPets.Where(pet => pet.CanSleep()).ToArray());
		_petList.PetClicked += AfterSelectPet;
	}

	private void AfterSelectPet(PetSaveInfo pet)
	{
		_petList.PetClicked -= AfterSelectPet;

		_spot.PutPet(pet);
		_dialoger.StartDialog(_sleepDialog);
		_sleepDialog.StoryEnded.AddListener(Complete);
	}

	public override void OnQuestFinish()
	{
		base.OnQuestFinish();
		_sleepDialog.StoryEnded.RemoveListener(Complete);
		PlayerDataContainer.MoraCount = 1600;
		PlayerDataContainer.GemsCount = 2000;
	}
}
