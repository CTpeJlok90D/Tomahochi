using UnityEngine;
using UnityEditor;
public class GameObjectPanelExpand
{
	[MenuItem("GameObject/UI/Custom/Button")]
	private static void CreateButton()
	{
		InstanceObject(GameObjectSingletones.Button);
	}

	[MenuItem("GameObject/UI/Custom/Window")]
	private static void CreateWindow()
	{
		InstanceObject(GameObjectSingletones.Window);
	}

	private static void InstanceObject(GameObject original)
	{
		Canvas canvas = MonoBehaviour.FindAnyObjectByType<Canvas>();
		GameObject instance = MonoBehaviour.Instantiate(original);
		instance.transform.SetParent(canvas.transform);
		if (Selection.activeGameObject != null)
		{
			instance.transform.SetParent(Selection.activeGameObject.transform);
			instance.transform.localPosition = Vector3.zero;
		}
	}
}
