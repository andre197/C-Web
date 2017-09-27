namespace Social_Network.Client
{
    public static class TagTransformator
    {
        public static string Transform(string tag)
        {

            if (!tag.StartsWith('#'))
            {
                tag = '#' + tag;
            }
            if (tag.Contains(" "))
            {
                tag = tag.Replace(" ", "");
            }
            if (tag.Length > 20)
            {
                tag = tag.Substring(0, 20);
            }

            return tag;
        }
    }
}
