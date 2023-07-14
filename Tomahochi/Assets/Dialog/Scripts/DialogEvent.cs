using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem
{
	[AddComponentMenu("Dialog/Dialog event")]
	public class DialogEvent : MonoBehaviour
    {
        public Dialog Dialog;
        public UnityEvent Event;

        public void OnEnable()
        {
            Dialog.StoryEnded.AddListener(Event.Invoke);
        }

		public void OnDisable()
		{
			Dialog.StoryEnded.RemoveListener(Event.Invoke);
		}
	}
}