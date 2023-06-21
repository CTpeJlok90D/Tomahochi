using UnityEngine;
using System;
using System.Collections.Generic;

namespace AICore
{
    [Serializable]
    public class Memory
    {
        [SerializeField] private List<Impact> _impact;
        [SerializeField] private bool _infinity; 
        [SerializeField] private float _dituration;

        private float _currentDituration;

        public float CurrentDituration => _currentDituration;

        public Memory(float dituration)
        {
            _dituration = dituration;
            _impact = new();
        }

		[Serializable]
		public struct Impact
		{
			public Task Task;
			public int priorityChagne;
		}

        public void UpdateDituration()
        {
			_currentDituration = _dituration;
		}

        public void OnMemoryAdd(Factor info)
        {
            UpdateDituration();
			foreach (Impact impact in _impact)
            {
                impact.Task.Priority += impact.priorityChagne;
                impact.Task.OnTakeNewInfo(info);
            }
        }

        public void UpdateInfo(Factor info)
        {
			foreach (Impact impact in _impact)
			{
				impact.Task.OnTakeNewInfo(info);
			}
		}

        public void OnUpdate()
        {
			if (_infinity)
            {
                return;
            }

			_currentDituration -= Time.deltaTime;
		}

        public void OnMemoryRemove()
        {
			foreach (Impact impact in _impact)
			{
				impact.Task.Priority -= impact.priorityChagne;
			}
		}
	}   
}