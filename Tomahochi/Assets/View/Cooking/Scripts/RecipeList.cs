using UnityEngine;

public class RecipeList : MonoBehaviour
{
	[SerializeField] private Transform _content;
	[SerializeField] private RecipeListElement _elementPrefab;
	[SerializeField] private Campfire _selectedCampfire;
	[SerializeField] private GameObject _recipeListUIParent;

	private void OnEnable()
	{
		Selecteble.SelectObjectChange += OnSelectedObjectChange;
	}

	private void OnDisable()
	{
		Selecteble.SelectObjectChange -= OnSelectedObjectChange;
	}

	public void OnSelectedObjectChange(Selecteble selecteble)
	{
		if (selecteble == null)
		{
			_recipeListUIParent.SetActive(false);
			return;
		}
		_selectedCampfire = selecteble.GetComponent<Campfire>();
		_recipeListUIParent.SetActive(_selectedCampfire != null);
		ShowRecipes(_selectedCampfire);
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
