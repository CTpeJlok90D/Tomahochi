using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace DialogSystem
{
	[AddComponentMenu("Dialog/Dialoger")]
	public class Dialoger : MonoBehaviour
    {
        [SerializeField] private Transform _dialogWindow;
        [SerializeField] private Image _characterImage;
        [SerializeField] private TMP_Text _dialogTextPlace;
        [SerializeField] private Transform _answerContainer;
        [SerializeField] private AnswerView _answerPrefub;
        [SerializeField] private bool _clearDealogTextBeforeShowAnswers = true;
        [SerializeField] private GameObject[] _onDialogDisabledObjects;
        [SerializeField] private Image[] _onDialogDiabledImages;


		private int _currentStoryNumber;
        private Dialog _currentDialog;
        private bool _chosingAnswers = false;

        public bool InDialog => _currentDialog != null;
        public bool ChosingAnswers => _chosingAnswers;

        public void FinishDialog()
        {
            foreach (GameObject obj in _onDialogDisabledObjects)
            {
                obj.SetActive(true);
            }
            foreach (Image image in _onDialogDiabledImages)
            {
                image.enabled = true;
			}
			_currentDialog = null;
            _characterImage.sprite = null;
            _dialogWindow.gameObject.SetActive(false);
        }

		public void StartDialog(Dialog dialog)
		{
			foreach (GameObject obj in _onDialogDisabledObjects)
			{
				obj.SetActive(false);
			}
			foreach (Image image in _onDialogDiabledImages)
			{
				image.enabled = false;
			}
			_chosingAnswers = false;
			_currentDialog = dialog;
			_currentStoryNumber = -1;
			foreach (Transform answer in _answerContainer)
			{
				Destroy(answer.gameObject);
			}
			_dialogWindow.gameObject.SetActive(true);
			NextStory();
		}

		public void NextStory()
        {
            if (ChosingAnswers)
            {
                return;
            }
            _currentStoryNumber++;
            if (_currentStoryNumber >= _currentDialog.Storys.Count)
            {
                UnityEvent endEvent = _currentDialog.StoryEnded;
                if (_currentDialog.Answers.Count == 0)
                {
                    FinishDialog();
					endEvent.Invoke();
					return;
                }
                endEvent.Invoke();
				ShowAnswers(_currentDialog.Answers);
                return;
            }

            _dialogTextPlace.text = _currentDialog.Storys[_currentStoryNumber].Text;
            Sprite newSprite = _currentDialog.Storys[_currentStoryNumber].IntercolutorSprite;
			if (newSprite != null)
            {
                _characterImage.sprite = newSprite;
            }
        }

        private void ShowAnswers(List<Answer> answers)
        {
            if (_clearDealogTextBeforeShowAnswers)
            {
				_dialogTextPlace.text = "";
			}
            int currentAnswerNumber = 0;
            _chosingAnswers = true;
            foreach (Answer answer in answers)
            {
                int copyCurrentAnswerNumber = currentAnswerNumber;
                AnswerView view = Instantiate(_answerPrefub, _answerContainer).Init(answer);
				view.Chosen.AddListener(() => StartDialog(_currentDialog.Answers[copyCurrentAnswerNumber].NextDialog));
                currentAnswerNumber++;
            }
        }
    }

}