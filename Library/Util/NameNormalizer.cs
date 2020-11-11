using System.Text;

namespace LibraryNet2020.Util
{
    public class NameNormalizer
	{
        public string Normalize(string unnormalizedName)
        {
            var parts = Parts(unnormalizedName);
            if (parts.Length == 1)
                return unnormalizedName;
            else if (parts.Length == 2)
            {
                return Last(parts) + ", " + First(parts);
            }

            return Last(parts) + ", " + First(parts) + " " + GetMiddleNames(parts);            
        }

        private static string GetMiddleNames(string[] parts)
        {
            StringBuilder middleNameInitials = new StringBuilder();
            
            for (int i = 1; i < parts.Length-1; i++)
            {
                middleNameInitials.Append($"{parts[i][0]}. ");
            }

            return middleNameInitials.ToString().Trim();
        }

        private string[] Parts(string name)
        {
            return name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        }           
        private static string First(string[] parts)
        {
            return parts[0];
        }

        private static string Last(string[] parts)
        {
            return parts[parts.Length - 1];
        }
    }
}