namespace TemplateGenerator.Extensions
{
    public static class StringExtensions
    {
        public static string ToNonTralingSlashString(this string path)
        {
            if (path.EndsWith("/", System.StringComparison.Ordinal))
            {
                return path.Substring(0, path.Length - 1);
            }

            return path;
        }

        public static string ToTralingSlashString(this string path)
        {
            if (!path.EndsWith("/", System.StringComparison.Ordinal))
            {
                return path + "/";
            }

            return path;
        }
    }
}
