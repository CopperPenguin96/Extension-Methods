namespace Extension_Methods
{
    public static class ByteMethods
    {
        public static string GetString(this byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}
