using System;
using UnityEngine;

[ExecuteAlways]
public class UI : MonoBehaviour
{
	[SerializeField] private UnityDictionarity<UIMode, ModeInfo> _tabPerMode;
	[SerializeField] private UIMode _mode;
	[SerializeField] private PetList _petList;
	[SerializeField] private PetUI _petUI;
	[SerializeField] private RecipeList _recipeList;

	public delegate void ModeChangedHandler(UIMode mode);
	private event ModeChangedHandler _modeChanged;
	public static event ModeChangedHandler ModeChanged
	{
		add => _instance._modeChanged += value;
		remove => _instance._modeChanged -= value;
	}
	public static UIMode Mode
	{
		get
		{
			return _instance._mode;
		}
		set
		{

			foreach (ModeInfo modeInfo in _instance._tabPerMode.Values)
			{
				modeInfo.Tab.SetActive(false);
			}
			ModeInfo info = _instance._tabPerMode[value];
			_instance._tabPerMode[value].Tab.SetActive(true);
			_instance._mode = value;
			_instance._modeChanged?.Invoke(Mode);
			MoveCameraPanel.Singletone.enabled = info.CanMoveCamera;
		}
	}
	private static UI _instance;
	public static PetList PetList => _instance._petList;
	public static PetUI PetUI => _instance._petUI;
	public static RecipeList RecipeList => _instance._recipeList;
	public static bool HaveInstance => _instance != null;
	private void Awake()
	{
#if UNITY_EDITOR
		if (Application.IsPlaying(gameObject))
		{
#endif
			_instance = this;
#if UNITY_EDITOR
		}
		else
		{
			EditorAwake();
		}
#endif
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

	[Serializable]
	private struct ModeInfo
	{
		public GameObject Tab;
		public bool CanMoveCamera;
	}
}
