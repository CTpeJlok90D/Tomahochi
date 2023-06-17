using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Cooking/Recipe")]
public class Recipe : ScriptableObject
{
	[SerializeField] private List<IngridiendData> _ingredients = new();
	[SerializeField] private Food _result;
	[SerializeField] private float _cookTime = 3f;
	[SerializeField] private Vector2 _acceptebleTime = new Vector2(1,2);
	[SerializeField] private int _cookCountToSkipMiniGame = 5;

	public List<IngridiendData> Igredients => new(_ingredients);
	public Food Result => _result;
	public float CookTime => _cookTime;
	public Vector2 AcceptebleTime => _acceptebleTime;
	public int CookCountToSkipMiniGame => _cookCountToSkipMiniGame;

	[Serializable]
	public struct IngridiendData
	{
		public Ingredient Ingredient;
		public int Count;
	}
}
