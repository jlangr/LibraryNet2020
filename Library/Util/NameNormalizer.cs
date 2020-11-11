using System.Linq;

namespace LibraryNet2020.Util
{
    public class NameNormalizer
	{
        public string Normalize(string unnormalizedName)
        {
            var parts = Parts(unnormalizedName.Trim());
            if (IsMononym(parts))
                return unnormalizedName;
            if (HasTwoMiddleNames(parts))
            {
                return Last(parts) + ", " + First(parts) + " " + Middle(parts) + " " + SecondMiddle(parts);
            }
            if (HasMiddleName(parts))
            {
                return Last(parts) + ", " + First(parts) + " " + Middle(parts);
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

        private static string Middle(string[] parts)
        {
            if(parts[1].Length == 1)
            {
                return $"{parts[1][0]}";
            }

            return  $"{parts[1][0]}.";
        }

        private static string SecondMiddle(string[] parts)
        {
            if (parts[2].Length == 1)
            {
                return $"{parts[2][0]}";
            }

            return $"{parts[2][0]}.";
        }


        private static bool HasMiddleName(string[] parts)
        {
            return parts.Length == 3;
        }

        private static bool HasTwoMiddleNames(string[] parts)
        {
            return parts.Length == 4;
        }
    }
}