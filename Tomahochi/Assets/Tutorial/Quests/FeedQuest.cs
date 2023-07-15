using DialogSystem;
using Saving;
using UnityEngine;

public class FeedQuest : Quest
{
	[SerializeField] private Dialoger _dialoger;
	[SerializeField] private Dialog _startDialog;
	[SerializeField] private Dialog _feedHint;
	[SerializeField] private GameObject _foodList;
	[SerializeField] private GameObject _petSelectHint; 
	[SerializeField] private GameObject _feedObjecthint;

	public override void OnQuestBegin()
	{
		base.OnQuestBegin(); 

		_dialoger.StartDialog(_startDialog);
		_foodList.SetActive(true);
		PlayerDataContainer.FoodCountChanged.AddListener(OnFoodCountChanged);
		Selecteble.SelectObjectChange += OnPetSelect;
		_petSelectHint.SetActive(true);
	}

	private void OnPetSelect(Selecteble selecteble)
	{
		if (selecteble == null || selecteble.TryGetComponent(out PetView view) == false)
		{
			return;
		}
		Selecteble.SelectObjectChange -= OnPetSelect;

		_petSelectHint.SetActive(false);
		_feedObjecthint.SetActive(true);
		_dialoger.StartDialog(_feedHint);
	}

	private void OnFoodCountChanged(Storageble item, int count)
	{
		Selecteble.SelectObjectChange -= OnPetSelect;

		_feedObjecthint.SetActive(false);
		Complete();
	}
}
