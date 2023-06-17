using System;
using UnityEngine;

namespace AICore
{
    public enum Factor
    {
        Player,
        StrangeSound,
        PlayerHided
    }

    [Serializable]
    public struct FactorInfo
    {
        public Factor Factor;
    }
}
