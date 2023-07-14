using UnityEngine;

namespace DialogSystem
{
    [AddComponentMenu("Dialog/Dialog container")]
    public class DialogContainer : MonoBehaviour
    {
        [SerializeField] private Dialog _dialog;

        public Dialog Dialog => _dialog;
    }
}