using System;

namespace DROMsM
{
    public static class FloatExtensions
    {
        public static float Clamp(this float self, float min, float max)
        {
            return Math.Min(max, Math.Max(self, min));
        }
    }
}