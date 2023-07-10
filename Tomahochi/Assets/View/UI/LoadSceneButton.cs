using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
	[SerializeField] private Button _button;
	[SerializeField] private GameObject _loadSceneIndicator;
	public int SceneNumber;

	private void OnEnable()
	{
		_button.onClick.AddListener(OnClick);
	}

	private void OnDisable()
	{
		_button.onClick.RemoveListener(OnClick);
	}

	private void OnClick()
	{
		PlayerDataContainer.SavePlayerData();
		SceneManager.LoadSceneAsync(SceneNumber);
		_loadSceneIndicator.SetActive(true);
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		_button ??= GetComponent<Button>();
	}
#endif
}
