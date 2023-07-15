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
	[SerializeField] private GameObject _exitEditModeHint;
	[SerializeField] private InWorldHint _worldBedHint;
	[SerializeField] private GameObject _selectPetHint;

	private SleepSpotView _spot;

	public override void OnQuestBegin()
	{
		base.OnQuestBegin();
		_dialoger.StartDialog(_startDialog);
		_startDialog.StoryEnded.AddListener(OnStartDialogEnd);
		Selecteble.SelectObjectChange += OnSelectObject;
		_exitBuildButton.onClick.AddListener(OnExitBuild);
	}

	private void OnStartDialogEnd()
	{
		_exitEditModeHint.SetActive(true);
	}

	private void OnExitBuild()
	{
		_exitEditModeHint.SetActive(false);
		_worldBedHint.Target = FindObjectOfType<SleepSpotView>().transform;
		_worldBedHint.gameObject.SetActive(true);
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

		_worldBedHint.gameObject.SetActive(false);
		_selectPetHint.SetActive(true);
		_dialoger.StartDialog(_layPetSleep);
		_petList.Show(PlayerDataContainer.UnlockedPets.Where(pet => pet.CanSleep()).ToArray());
		_petList.PetClicked += AfterSelectPet;
	}

	private void AfterSelectPet(PetSaveInfo pet)
	{
		_petList.PetClicked -= AfterSelectPet;

		_selectPetHint.SetActive(false);
		_spot.PutPet(pet);
		_dialoger.StartDialog(_sleepDialog);
		_sleepDialog.StoryEnded.AddListener(OnComplete);
	}

	private void OnComplete()
	{
		_sleepDialog.StoryEnded.RemoveListener(OnComplete);

		PlayerDataContainer.MoraCount = 1600;
		PlayerDataContainer.GemsCount = 2000;
		PlayerDataContainer.SavePlayerData();
		Complete();
	}
}
