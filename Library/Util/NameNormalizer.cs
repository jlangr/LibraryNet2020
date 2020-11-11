using System.Linq;
using LibraryNet2020.Migrations;

namespace LibraryNet2020.Util
{
    public class NameNormalizer
	{
        public string Normalize(string unnormalizedName)
        {
            var parts = Parts(unnormalizedName.Trim());
            if (IsMononym(parts))
                return unnormalizedName;
            if (IsDuonym(parts))
                return Last(parts) + ", " + First(parts);
            return Last(parts) + ", " + First(parts) + " " + MiddleInitial(parts);
        }

        private string MiddleInitial(string[] parts)
        {
            return $"{parts[1][0]}.";
        }

        private bool IsDuonym(string[] parts)
        {
            return parts.Length == 2;
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
            return parts.Last();
        }
    }
}