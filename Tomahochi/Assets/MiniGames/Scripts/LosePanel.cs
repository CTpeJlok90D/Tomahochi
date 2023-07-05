using Saving;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : MonoBehaviour
{
	[SerializeField] private TMP_Text _scoreCaption;
	[SerializeField] private TMP_Text _bestScoreCaption;
	[SerializeField] private TMP_Text _newRecordText;

	private string _scoreFormat;
	private string _bestScoreFormat;

	protected virtual void Awake()
	{
		_scoreFormat = _scoreCaption.text;
		_bestScoreFormat = _bestScoreCaption.text;
	}

	public void Show(int score)
	{
		gameObject.SetActive(true);
		int miniGameIndex = SceneManager.GetActiveScene().buildIndex;
		float bestScore = PlayerDataContainer.GetRecord(miniGameIndex);

		_scoreCaption.text = string.Format(_scoreFormat, (int)score);
		_bestScoreCaption.text = string.Format(_bestScoreFormat, (int)bestScore);

		if (bestScore < score)
		{
			_newRecordText.gameObject.SetActive(true);
			PlayerDataContainer.WriteRecord(miniGameIndex, score);
		}
	}
}
