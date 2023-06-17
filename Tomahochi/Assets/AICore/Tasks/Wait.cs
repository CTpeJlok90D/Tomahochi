using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace AICore.Tasks
{
	public class Wait : Task
	{
		[SerializeField] private float _waitTime;
		[SerializeField] private UnityEvent _waitCompleted;

		public override void OnBegin()
		{
			base.OnBegin();
			StartCoroutine(WaitCoroutine(_waitTime));
			Agent.isStopped = true;
		}

		public override void OnCancel()
		{
			base.OnCancel();
			Agent.isStopped = false;
		}

		private IEnumerator WaitCoroutine(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			_waitCompleted.Invoke();
		}
	}
}
