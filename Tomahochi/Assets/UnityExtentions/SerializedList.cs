using System;
using System.Collections.Generic;

namespace UnityExtentions
{
    [Serializable]
    public class SerializedList<T>
    {
        public List<T> List = new();
    }
}
