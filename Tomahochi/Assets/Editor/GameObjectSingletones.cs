using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[CreateAssetMenu(menuName = "Editor/UIObjects")]
public class GameObjectSingletones : ScriptableObject
{
	[SerializeField] private GameObject _button;
	[SerializeField] private GameObject _window;

	public static GameObject Button => _instance._button;
	public static GameObject Window => _instance._window;


	private static GameObjectSingletones _instance;
	public static GameObjectSingletones Instance => _instance;

	private void OnEnable()
	{
		_instance = this;
	}
}
