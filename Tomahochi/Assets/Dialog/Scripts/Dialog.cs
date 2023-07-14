using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem
{
    [CreateAssetMenu(menuName = "Dialog/Dialog")]
    public class Dialog : ScriptableObject
    {
        [SerializeField] private List<Story> _storys = new();
        [SerializeField] private List<Answer> _answers = new();
        private UnityEvent _storyEnded = new();
        public List<Story> Storys => _storys;
        public List<Answer> Answers => _answers;
        public UnityEvent StoryEnded => _storyEnded;
    }
}