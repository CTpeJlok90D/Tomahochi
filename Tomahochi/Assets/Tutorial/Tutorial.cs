using Memory;
using Saving;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	[SerializeField] private QuestLine _tutorialLine;
	[SerializeField] private QuestLine _linePart2;
	[SerializeField] private GameObject[] _onTutorial1DisabledElements;
	[SerializeField] private GameObject[] _onTutorial2DisabledElements;
	public const string SAVE_KEY = "Tutorial";
	private void Awake()
	{
		int progress = new Load<Progress>(SAVE_KEY).Data.Value;

		if (progress == 0)
		{
			foreach (GameObject obj in _onTutorial1DisabledElements)
			{
				obj.SetActive(false);
			}
			_tutorialLine.StartLine();
			return;
		}
		if (progress == 1)
		{
			foreach (GameObject obj in _onTutorial2DisabledElements)
			{
				obj.SetActive(false);
			}
			_linePart2.StartLine();
			return;
		}
		if (progress == 2)
		{
			Destroy(gameObject);
			return;
		}
	}

	private void OnEnable()
	{
		_linePart2.Complited.AddListener(OnLineComplete);
	}

	private void OnDisable()
	{
		_linePart2.Complited.RemoveListener(OnLineComplete);
	}

	private void OnLineComplete()
	{
		new Save(SAVE_KEY, new Progress(2));
		foreach (GameObject obj in _onTutorial1DisabledElements)
		{
			obj.SetActive(true);
		}
		Furniture furniture = FindObjectOfType<FurnitureView>().Source;
		PlayerDataContainer.PlaceFurniture(furniture);
		Destroy(gameObject);
	}

	public struct Progress
	{
		public int Value;

		public Progress(int value)
		{
			Value = value;
		}
	}
}
