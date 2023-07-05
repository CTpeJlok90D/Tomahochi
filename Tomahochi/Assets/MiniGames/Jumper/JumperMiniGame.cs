using UnityEngine;
using UnityEngine.Events;

public class JumperMiniGame : MiniGame
{
	[SerializeField] private JumpPlatform[] _platforms;
	[SerializeField] private JumperPlayer _player;


	private void Awake()
	{
		IncreaseJoy = true;
		IncreaseXP = true;
	}

	private void OnEnable()
	{
		foreach (JumpPlatform platform in _platforms)
		{
			platform.LinkGenerated += OnGenerate;
		}

		_player.Die += OnPlayerDie;
	}

	private void OnDisable()
	{
		foreach (JumpPlatform platform in _platforms)
		{
			platform.LinkGenerated -= OnGenerate;
		}

		_player.Die -= OnPlayerDie;
	}

	public void OnGenerate()
	{
		Score++;
	}

	public void OnPlayerDie()
	{
		EndGame();
		IncreaseJoy = false;
		IncreaseXP = false;
	}
}
