using UnityEngine;

namespace UnityExtentions
{
    public static class MonoBehaviorExtentions
    {
        public static bool TryStopCoroutine(this MonoBehaviour @this, Coroutine coroutine)
        {
            if (coroutine == null) 
            {
                return false;
            }
            @this.StopCoroutine(coroutine);
            return true;
        }
    }
}
