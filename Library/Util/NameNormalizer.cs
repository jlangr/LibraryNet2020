namespace LibraryNet2020.Util
{
    public class NameNormalizer
	{
        public string Normalize(string unnormalizedName)
        {
            var parts = Parts(unnormalizedName);
            if (IsMononym(parts))
                return unnormalizedName;
            return Last(parts) + ", " + First(parts);
        }

        private string[] Parts(string name)
        {
            return name.Split(' ');
        }

        private static bool IsMononym(string[] parts)
        {
            return parts.Length != 2;
        }

        private static string First(string[] parts)
        {
            return parts[0];
        }

        private static string Last(string[] parts)
        {
            return parts[1];
        }
    }
}