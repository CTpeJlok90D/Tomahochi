using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class UI : MonoBehaviour
{
	[SerializeField] private PetList _petList;
	[SerializeField] private PetUI _petUI;
	[SerializeField] private RecipeList _recipeList;

	private static UI _instance;
	public static PetList PetList => _instance._petList;
	public static PetUI PetUI => _instance._petUI;
	public static RecipeList RecipeList => _instance._recipeList;

	private void Awake()
	{
#if UNITY_EDITOR
		if (Application.IsPlaying(gameObject))
		{
#endif
			GameAwake();
#if UNITY_EDITOR
		}
		else
		{
			EditorAwake();
		}
#endif
	}

	private void GameAwake()
	{
		_instance = this;
	}
#if UNITY_EDITOR
	private void EditorAwake()
	{
		UI[] instances = FindObjectsOfType<UI>();
		if (instances.Length > 1)
		{
			DestroyImmediate(this);
			Debug.LogWarning("UI conponent is already created in this scene!", FindObjectOfType<UI>());
			return;
		}
	}
#endif
}
