using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AICore
{
    [Serializable]
    public class Patrol : Task
    {
        [Header("Patrol")]
        [SerializeField] private List<Transform> _patrolPoints;
        [SerializeField] private UnityEvent<Transform> _pointCame = new();

        private int _currentPointIndex = 0;

        public Transform CurrentPoint => _patrolPoints[CurrentPointIndex];
        public UnityEvent<Transform> PointCame => _pointCame;


        private int CurrentPointIndex
        {
            get
            {
                return _currentPointIndex;
            }
            set
            {
                _currentPointIndex = value;
				if (_currentPointIndex >= _patrolPoints.Count)
                {
                    _currentPointIndex = 0;
                    return;
				}
                if (_currentPointIndex < 0)
                {
                    _currentPointIndex = _patrolPoints.Count - 1;
                    return;
				}
            }
        }

		public override void OnUpdate()
        {
            base.OnUpdate();
			if (Vector3.Distance(Agent.transform.position, CurrentPoint.position) <= Agent.stoppingDistance)
            {
                CurrentPointIndex = UnityEngine.Random.Range(0,_patrolPoints.Count);
                _pointCame.Invoke(CurrentPoint);
			}
			Agent.destination = CurrentPoint.position;
		}
    }
}