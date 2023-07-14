using Saving;
using UnityEngine;

public class Campfire : MonoBehaviour
{
	[SerializeField] private CookMiniGame _cookMiniGame;
	[SerializeField] private Recipe _selectedRecipe;
	public delegate void RecipeSelectedDelegate(Recipe recipe);
	private event RecipeSelectedDelegate _selectedRecipeChanged;
	private bool _isCooking = false;


	public bool IsCooking => _isCooking;
	public Recipe SelectedRecipe => _selectedRecipe;
	public event RecipeSelectedDelegate SelectedRecipeChanged
	{
		add => _selectedRecipeChanged += value;
		remove => _selectedRecipeChanged -= value;
	}



	public void SelectRecipe(Recipe recipe)
	{
		_selectedRecipe = recipe;
		_selectedRecipeChanged?.Invoke(recipe);
	}
	public bool CanAutoCook() => PlayerDataContainer.GetCookedFoodCount(_selectedRecipe.Result) >= _selectedRecipe.CookCountToSkipMiniGame && CanCook();
	public bool CanCook()
	{
		if (IsCooking || _selectedRecipe == null)
		{
			return false;
		}
		foreach (Recipe.IngridiendData data in _selectedRecipe.Igredients)
		{
			if (PlayerDataContainer.GetIngridientCount(data.Ingredient) < data.Count)
			{
				return false;
			}
		}
		return true;
	}

	public void AutoCook()
	{
		if (CanAutoCook() == false)
		{
			return;
		}

		foreach (Recipe.IngridiendData data in _selectedRecipe.Igredients)
		{
			PlayerDataContainer.RemoveIngridient(data.Ingredient, data.Count);
		}
		PlayerDataContainer.AddFood(_selectedRecipe.Result);
	}

	public void HandCook()
	{
		if (CanCook() == false)
		{
			return;
		}
		_isCooking = true;

		foreach (Recipe.IngridiendData data in _selectedRecipe.Igredients)
		{
			PlayerDataContainer.RemoveIngridient(data.Ingredient, data.Count);
		}

		_cookMiniGame.GameEnded += OnMiniGameEnd;
		_cookMiniGame.SetUpSettings(_selectedRecipe.CookTime, _selectedRecipe.AcceptebleTime);
		_cookMiniGame.StartGame();
	}
	private void OnMiniGameEnd(CookMiniGame.Result result)
	{
		_isCooking = false;
		if (result == CookMiniGame.Result.Success)
		{
			PlayerDataContainer.AddCookedFood(_selectedRecipe.Result);
		}
		
		_cookMiniGame.GameEnded -= OnMiniGameEnd;
	}
}