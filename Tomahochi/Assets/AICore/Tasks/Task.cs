using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace AICore
{
    public class Task : MonoBehaviour
    {
        [Header("Task")]
		[SerializeField] private NavMeshAgent _agent;
        public int Priority;
		[SerializeField] private float _maxSpeed = 4f;
		[SerializeField] private float _acceleration = 8;
		[SerializeField] private float _soppingDistance = 0f;
        [SerializeField] private UnityEvent _begined;
        [SerializeField] private UnityEvent _ended;

		private FactorInfo _info;

		public NavMeshAgent Agent => _agent;
		public FactorInfo Info => _info;
        public UnityEvent Begined => _begined;
        public UnityEvent Ended => _ended;

        public virtual void OnBegin() 
        {
			_agent.speed = _maxSpeed;
			_agent.acceleration = _acceleration;
			_agent.stoppingDistance = _soppingDistance;
            _begined.Invoke();
		}
        public virtual void OnUpdate() { }       
        public virtual void OnCancel() 
        {
            _ended.Invoke();
		}
        public virtual void OnTakeNewInfo(FactorInfo info) 
        {
            _info = info;
        }
    }
}