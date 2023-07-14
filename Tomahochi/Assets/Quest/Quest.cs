using UnityEngine;
using UnityEngine.Events;

public abstract class Quest : MonoBehaviour
{
	[SerializeField] private UnityEvent _questStarted;
	[SerializeField] private UnityEvent _questEnded;

	private bool _questIsStarted;
	public UnityEvent QuestStarted => _questStarted;
	public UnityEvent QuestEnded => _questEnded;
	public bool QuestIsStarted => _questIsStarted;

	public virtual void OnQuestFinish() { }
	public virtual void OnQuestBegin()
	{
		_questIsStarted = true;
		_questStarted.Invoke();
	}
	public void Complete()
	{
		OnQuestFinish();
		_questIsStarted = false;
		_questEnded.Invoke();
	}
}