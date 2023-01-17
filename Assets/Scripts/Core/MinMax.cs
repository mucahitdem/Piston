using System;

namespace Core
{
    [Serializable]
    public class MinMax
    {
        public readonly float MinVal;
        public readonly float MaxVal;

        public MinMax(float minVal, float maxVal)
        {
            MinVal = minVal;
            MaxVal = maxVal;
        }
    }
}