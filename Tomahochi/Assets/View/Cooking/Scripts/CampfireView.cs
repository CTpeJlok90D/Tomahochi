using UnityEngine;

public class CampfireView : MonoBehaviour
{
	[SerializeField] private Campfire _campfire;
	[SerializeField] private UnityDictionarity<Ingredient, RecipeIngridientCostView> _ingridientsView;
	[SerializeField] private RecipeIngridientCostView _recipeViewPrefab;
	[SerializeField] private Transform _content;

	Ingredient[] _ingredients;

	private void Awake()
	{
		_ingredients = Resources.LoadAll<Ingredient>("");
		foreach (Ingredient ingridient in _ingredients)
		{
			_ingridientsView.Add(ingridient, Instantiate(_recipeViewPrefab, _content));
			_ingridientsView[ingridient].Ingredient = ingridient;
			_ingridientsView[ingridient].Count = 0;
		}
	}

	private void OnEnable()
	{
		_campfire.SelectedRecipeChanged += OnSelectedRecipeChange;
	}

	private void OnDisable()
	{
		_campfire.SelectedRecipeChanged -= OnSelectedRecipeChange;
	}

	private void OnSelectedRecipeChange(Recipe recipe)
	{
		foreach (Ingredient ingredient in _ingredients)
		{
			_ingridientsView[ingredient].Count = 0;
		}
		foreach (Recipe.IngridiendData info in recipe.Igredients)
		{
			_ingridientsView[info.Ingredient].Count = info.Count;
		}
	}
}
