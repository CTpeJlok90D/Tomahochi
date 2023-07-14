using System;
using UnityEngine;

namespace DialogSystem
{
    [Serializable]
    public struct Story
    {
        [TextArea(3,50)]
        public string Text;
        public Sprite IntercolutorSprite;
    }
}
