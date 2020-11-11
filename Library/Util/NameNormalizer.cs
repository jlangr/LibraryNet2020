using System.Linq;
using System.Text;

namespace LibraryNet2020.Util
{
    public class NameNormalizer
	{
        public string Normalize(string unnormalizedName)
        {
            var parts = Parts(unnormalizedName.Trim());
            if (IsMononym(parts))
                return unnormalizedName;
            if (HasMiddleNames(parts))
            {
                return Last(parts) + ", " + First(parts) + MiddleNames(parts);
            }
            return Last(parts) + ", " + First(parts);
        }

        private string[] Parts(string name)
        {
            return name.Split(' ');
        }

        private static bool IsMononym(string[] parts)
        {
            return parts.Length == 1;
        }

        private static string First(string[] parts)
        {
            return parts[0];
        }

        private static string Last(string[] parts)
        {
           
            return parts.Last();
        }

        private static string MiddleNames(string[] parts)
        {
            var ret = new StringBuilder();
            for (int i = 1; i < parts.Length - 2; i++)
            {
                if (parts[i].Length == 1)
                {
                    ret.Append($" {parts[i][0]}");
                }
                else
                {
                    ret.Append($" {parts[i][0]}.");
                }
            }
            return ret.ToString();
        }

        private static bool HasMiddleNames(string[] parts)
        {
            return parts.Length > 2;
        }
    }
}