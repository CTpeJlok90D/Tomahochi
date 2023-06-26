using System;

namespace UnityExtentions
{
    [Serializable]
    public struct Range
    {
        public float Max;
        public float Min;

        public Range(float min = -1, float max = 1)
        {
            Min = min;
            Max = max;
        }


        public static implicit operator float(Range range)
        {
            return range.RandomValue();
        }

		public override string ToString()
		{
            return RandomValue().ToString();
		}

		public float RandomValue()
		{
            return UnityEngine.Random.Range(Min, Max);
		}

        public bool Contains(float value)
        {
            return value >= Min && value <= Max;
        }
    }
}
