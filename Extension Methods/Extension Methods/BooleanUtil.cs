namespace Extension_Methods
{
    public static class BooleanUtil
    {
        public static byte ToByte(this bool bo)
        {
            return bo ? (byte)1 : (byte)0;
        }
    }
}
