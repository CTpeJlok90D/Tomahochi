using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeListElement : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private Recipe _recipe;
	[SerializeField] private Campfire _campfire;
	[SerializeField] private Image _image;
	[SerializeField] private TMP_Text _nameCaption;
	[SerializeField] private float _cantCookAlpha = 0.6f;

	private bool _initialized = false;

	public Recipe Recipe => _recipe;

	public RecipeListElement Init(Recipe recipe, Campfire campfire)
	{
		_initialized = true;
		_recipe = recipe;
		_campfire = campfire;
		return this;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		_campfire.Cook(_recipe);
		OnStorageCountChange();
	}

	private void OnStorageCountChange()
	{
		bool value = true;
		foreach (Recipe.IngridiendData data in _recipe.Igredients)
		{
			if (data.Ingredient.GetStorageCount() < data.Count)
			{
				value = false;
				break;
			}
		}
		if (value)
		{
			_image.color = Color.white;
		}
		else
		{
			_image.color = new(1, 1, 1, _cantCookAlpha);
		}
	}

	public IEnumerator Start()
	{
		yield return null;
		if (_initialized == false)
		{
			throw new Exception("Object was not initialized!");
		}
		_image.sprite = _recipe.Result.ViewSprite;
		_nameCaption.text = _recipe.Result.ViewName;
		if (_campfire.CanCook(_recipe) == false)
		{
			_image.color = new(1,1,1, _cantCookAlpha);
		}
	}
}
