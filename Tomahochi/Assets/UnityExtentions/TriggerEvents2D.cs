using UnityEngine;

namespace UnityExtentions
{
    public class TriggerEvents2D : MonoBehaviour
    {
        public delegate void Handler(Collider2D collider);
        private event Handler _triggerEntered;
        private event Handler _triggerStaying;
        private event Handler _triggerExited;

        public event Handler TriggerEntered
        {
            add => _triggerEntered += value;
            remove => _triggerEntered -= value;
        }
        public event Handler TriggerStaying
        {
            add => _triggerStaying += value;
            remove => _triggerStaying -= value;
        }
        public event Handler TriggerExited
        {
            add => _triggerExited += value; 
            remove => _triggerExited -= value;
        }

		private void OnTriggerEnter2D(Collider2D collision)
		{
            _triggerEntered?.Invoke(collision);
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
            _triggerStaying?.Invoke(collision);
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
            _triggerExited?.Invoke(collision);
		}
	}
}
