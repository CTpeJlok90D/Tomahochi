using Saving;
using UnityEngine;
using UnityEngine.UI;

public class RecipeList : MonoBehaviour
{
	[SerializeField] private Transform _content;
	[SerializeField] private RecipeListElement _elementPrefab;
	[SerializeField] private Campfire _selectedCampfire;
	[SerializeField] private CookMiniGame _miniGame;
	[SerializeField] private GameObject _cookButtonsParent;
	[SerializeField] private GameObject _recipeListUIParent;
	[SerializeField] private Button _autoCookButton;
	[SerializeField] private Button _handCookButton;

	private void OnEnable()
	{
		Selecteble.SelectObjectChange += OnSelectedObjectChange;
		_miniGame.GameStarted += OnMiniGameStarted;
		_miniGame.GameEnded += OnMiniGameEnded;
		_autoCookButton.onClick.AddListener(OnAutoCookClick);
		_handCookButton.onClick.AddListener(OnHandCookClick);
		UpdateCookButtons();
	}

	private void OnDisable()
	{
		Selecteble.SelectObjectChange -= OnSelectedObjectChange;
		_miniGame.GameStarted -= OnMiniGameStarted;
		_miniGame.GameEnded -= OnMiniGameEnded;
		_autoCookButton.onClick.RemoveListener(OnAutoCookClick);
		_handCookButton.onClick.AddListener(OnHandCookClick);
	}

	private void UpdateAutoCookButton(Recipe recipe) => UpdateCookButtons();
	private void UpdateCookButtons()
	{
		if (_selectedCampfire == null || _selectedCampfire.SelectedRecipe == null)
		{
			_autoCookButton.interactable = false;
			_handCookButton.interactable = false;
			return;
		}
		_autoCookButton.interactable = _selectedCampfire.CanAutoCook();
		_handCookButton.interactable = _selectedCampfire.CanCook();
	}

	public void OnHandCookClick()
	{
		_selectedCampfire.HandCook();
	}
	public void OnAutoCookClick()
	{
		_selectedCampfire.AutoCook();
		UpdateCookButtons();
	}

	private void OnMiniGameStarted()
	{
		_cookButtonsParent.SetActive(false);
	}

	private void OnMiniGameEnded(CookMiniGame.Result result)
	{
		_cookButtonsParent.SetActive(true);
		UpdateCookButtons();
	}

	public void OnSelectedObjectChange(Selecteble selecteble)
	{
		if (selecteble == null)
		{
			if (_selectedCampfire != null)
			{
				_selectedCampfire.SelectedRecipeChanged -= UpdateAutoCookButton;
			}
			_recipeListUIParent.SetActive(false);
			return;
		}
		_selectedCampfire = selecteble.GetComponent<Campfire>();
		_recipeListUIParent.SetActive(_selectedCampfire != null);

		if (_selectedCampfire == null)
		{
			return;
		}
		_selectedCampfire.SelectedRecipeChanged += UpdateAutoCookButton;
		ShowRecipes(_selectedCampfire);
		UpdateCookButtons();
	}

	public void ShowRecipes(Campfire campfire = null)
	{
		campfire ??= _selectedCampfire;
		
		foreach (Transform child in _content.transform)
		{
			Destroy(child.gameObject);
		}

		Recipe[] recipes = Resources.LoadAll<Recipe>("");
		foreach (Recipe recipe in recipes)
		{
			Instantiate(_elementPrefab, _content.transform).Init(recipe, campfire);
		}
	}
}
