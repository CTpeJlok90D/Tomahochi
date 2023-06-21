using Saving;
using UnityEditor;
using UnityEngine;

public class Campfire : MonoBehaviour
{
	[SerializeField] private CookMiniGame _cookMiniGame;
	[SerializeField] private Recipe _cookingRecipe;

	public bool IsCooking => _cookingRecipe != null;

	public bool CanCook(Recipe recipe)
	{
		if (IsCooking)
		{
			return false;
		}
		foreach (Recipe.IngridiendData data in recipe.Igredients)
		{
			if (PlayerDataContainer.GetIngridientCount(data.Ingredient) < data.Count)
			{
				return false;
			}
		}
		return true;
	}
	public void Cook(Recipe recipe)
	{
		if (CanCook(recipe) == false)
		{
			return;
		}

		_cookingRecipe = recipe;

		foreach (Recipe.IngridiendData data in _cookingRecipe.Igredients)
		{
			PlayerDataContainer.RemoveIngridient(data.Ingredient, data.Count);
		}

		if (PlayerDataContainer.GetFoodCount(recipe.Result) >= recipe.CookCountToSkipMiniGame)
		{
			PlayerDataContainer.AddCookedFood(_cookingRecipe.Result);
			_cookingRecipe = null;
			return;
		}

		_cookMiniGame.GameEnded += OnMiniGameEnd;
		_cookMiniGame.SetUpSettings(_cookingRecipe.CookTime, _cookingRecipe.AcceptebleTime);
		_cookMiniGame.StartGame();
	}

	private void OnMiniGameEnd(CookMiniGame.Result result)
	{
		if (result == CookMiniGame.Result.Success)
		{
			PlayerDataContainer.AddCookedFood(_cookingRecipe.Result);
		}
		
		_cookingRecipe = null;
		_cookMiniGame.GameEnded -= OnMiniGameEnd;
	}

#if UNITY_EDITOR
	[Header("Editor only")]
	[SerializeField] private Recipe _cookRecipeForButton;

	[CustomEditor(typeof(Campfire))]
	public class CampfireEditor : Editor
	{
		public new Campfire target => base.target as Campfire;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (Application.isPlaying == false)
			{
				return;
			}
			if (target._cookRecipeForButton != null)
			{
				foreach (Recipe.IngridiendData ingredient in target._cookRecipeForButton.Igredients)
				{
					GUILayout.Label($"Request a {ingredient.Count} {ingredient.Ingredient}. On storage: {PlayerDataContainer.GetIngridientCount(ingredient.Ingredient)}");
				}
			}
			if (GUILayout.Button("Cook"))
			{
				target.Cook(target._cookRecipeForButton);
			}
		}
	}
#endif
}