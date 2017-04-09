namespace Waid
{
    public static class StringUtils
    {
        public static uint HashString(string s)
        {
            uint hash = 0;

            foreach (char c in s)
            {
                hash += c;
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }

            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);
            return hash;
        }
    }
}
