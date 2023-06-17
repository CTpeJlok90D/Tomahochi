using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AICore
{
    public class AI : MonoBehaviour
    {
        [SerializeField] private List<Task> _tasks = new();

        public List<Task> Tasks => new(_tasks);

        private Task currentTask;
        private Task GetPreferentTask()
        {
            Task preferentTask = _tasks[0];
            foreach (Task task in _tasks)
            {
                if (task.Priority > preferentTask.Priority)
                {
                    preferentTask = task;
                }
            }
            return preferentTask;
        }

        private void Update()
        {
            Task newCurrentTask = GetPreferentTask();
            if (currentTask != newCurrentTask)
            {
                currentTask?.OnCancel();
                currentTask = newCurrentTask;
                currentTask.OnBegin();
            }
			currentTask.OnUpdate();
        }

        public void AddTask(Task task)
        {
            _tasks.Add(task);
        }
#if UNITY_EDITOR
		public void OnValidate()
		{
            _tasks.Clear();
			foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out Task task))
                {
                    _tasks.Add(task);
				}
            }
		}
#endif
	}
#if UNITY_EDITOR
	[CustomEditor(typeof(AI))]
    public class AIEditor : Editor
    {
        private new AI target => base.target as AI;
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
            target.OnValidate();
		}
	}
#endif
}