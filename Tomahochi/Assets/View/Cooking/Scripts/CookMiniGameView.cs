using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CookMiniGameView : MonoBehaviour
{
	[SerializeField] private Button _cookButton;
	[SerializeField] private CookMiniGame _cookMiniGame;
	[SerializeField] private Scrollbar _currentTimeBar;
	[SerializeField] private Scrollbar _acceptebleZoneBar;
	[SerializeField] private GameObject _parent;
	

	private void OnEnable()
	{
		_cookButton.onClick.AddListener(OnClick);
		_cookMiniGame.GameStarted += OnMiniGameStart;
		_cookMiniGame.GameEnded += OnMiniGameEnd;
	}

	private void OnDisable()
	{
		_cookButton.onClick.RemoveListener(OnClick);
		_cookMiniGame.GameStarted -= OnMiniGameStart;
		_cookMiniGame.GameEnded -= OnMiniGameEnd;
	}

	private void OnClick()
	{
		_cookMiniGame.OnPlayerClick();
	}

	private void OnMiniGameStart()
	{
		float scale = (_cookMiniGame.AcceptebleInterval[1] - _cookMiniGame.AcceptebleInterval[0]) / _cookMiniGame.TimeInterval;
		float offcet = (_cookMiniGame.AcceptebleInterval[1] + _cookMiniGame.AcceptebleInterval[0]) / 2 / _cookMiniGame.TimeInterval;

		_acceptebleZoneBar.size = scale;
		_acceptebleZoneBar.value = offcet;
		_parent.SetActive(true);
		StartCoroutine(InGameCoroutine());
	}

	private void OnMiniGameEnd(CookMiniGame.Result result)
	{
		_parent.SetActive(false);
	}

	private IEnumerator InGameCoroutine()
	{
		while (_cookMiniGame.IsLaunched)
		{
			_currentTimeBar.value = _cookMiniGame.CurrentTime / _cookMiniGame.TimeInterval;
			yield return null;
		}
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		_cookMiniGame ??= gameObject.GetComponent<CookMiniGame>();
	}
#endif
}
