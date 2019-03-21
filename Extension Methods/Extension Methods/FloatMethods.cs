namespace Extension_Methods
{
    public static class FloatMethods
    {
        public static float Lerp(this float start, float end, float amount)
        {
            float difference = end - start;
            float adjusted = difference * amount;
            return start + adjusted;
        }
    }
}
