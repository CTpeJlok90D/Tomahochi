using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem
{
	public class AnswerEvent : MonoBehaviour
	{
		[SerializeField] private Answer _answer;
		[SerializeField] private UnityEvent _chosen;

		public Answer Answer => _answer;
		public UnityEvent Chosen => _chosen;

		private void OnEnable()
		{
			_answer.Chosen.AddListener(_chosen.Invoke);
		}

		private void OnDisable()
		{
			_answer.Chosen.RemoveListener(_chosen.Invoke);
		}
	}
}