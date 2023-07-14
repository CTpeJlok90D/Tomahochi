using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem
{
    [CreateAssetMenu(menuName = "Dialog/Answer")]
    public class Answer : ScriptableObject
    {
        [SerializeField] private string _text;
        [SerializeField] private Dialog _nextDialog;

        private UnityEvent _chosen = new();

        public string Text => _text;
        public Dialog NextDialog => _nextDialog;
        public UnityEvent Chosen => _chosen;
    }
}
