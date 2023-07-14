using DialogSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[AddComponentMenu("Dealog/AnswerView")]
public class AnswerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _answerText;
    [SerializeField] private Button _chooseButton;

	private Answer _answer;

	public UnityEvent Chosen => _chooseButton.onClick;

    public AnswerView Init(Answer answer)
    {
		_answer = answer;

		_answerText.text = _answer.Text;
        _chooseButton.onClick.AddListener(OnClick);

		return this;
    }

	private void OnDisable()
	{
		_chooseButton.onClick.RemoveListener(OnClick);
	}

	private void OnClick()
	{
		_answer.Chosen.Invoke();
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		_chooseButton ??= GetComponent<Button>();
		_answerText ??= GetComponent<TMP_Text>();
	}
#endif
}
