using DialogSystem;
using Saving;
using UnityEngine;

public class FeedQuest : Quest
{
	[SerializeField] private Dialoger _dialoger;
	[SerializeField] private Dialog _startDialog;
	[SerializeField] private Dialog _feedHint;
	[SerializeField] private GameObject _foodList;

	public override void OnQuestBegin()
	{
		base.OnQuestBegin(); 

		_dialoger.StartDialog(_startDialog);
		_foodList.SetActive(true);
		PlayerDataContainer.FoodCountChanged.AddListener(OnFoodCountChanged);
		Selecteble.SelectObjectChange += OnPetSelect;
	}

	private void OnPetSelect(Selecteble selecteble)
	{
		if (selecteble == null || selecteble.TryGetComponent(out PetView view) == false)
		{
			return;
		}
		_dialoger.StartDialog(_feedHint);
		Selecteble.SelectObjectChange -= OnPetSelect;
	}

	private void OnFoodCountChanged(Storageble item, int count)
	{
		Selecteble.SelectObjectChange -= OnPetSelect;
		Complete();
	}
}
