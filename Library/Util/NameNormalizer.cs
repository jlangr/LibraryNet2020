using System.Text;

namespace LibraryNet2020.Util
{
    public class NameNormalizer
    {
        public string Normalize(string unnormalizedName)
        {
            var parts = SplitByComma(unnormalizedName);
            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i] == "Jr.")
                {

                }
                else
                {
                    var regularName = SplitBySpace(unnormalizedName);

                    if (regularName.Length == 1)
                        return unnormalizedName;
                    else if (regularName.Length == 2)
                    {
                        return Last(regularName) + ", " + First(regularName);
                    }

                    return Last(regularName) + ", " + First(regularName) + " " + GetMiddleNames(regularName);

                }
            }
        }

        private static string GetMiddleNames(string[] parts)
        {
            StringBuilder middleNameInitials = new StringBuilder();

            for (int i = 1; i < parts.Length - 1; i++)
            {
                if (parts[i].Length > 1)
                    middleNameInitials.Append($"{parts[i][0]}. ");
                else
                    middleNameInitials.Append($"{parts[i][0]} ");
            }

            return middleNameInitials.ToString().Trim();
        }

        private string[] SplitBySpace(string name)
        {
            return name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        }

        private string[] SplitByComma(string name)
        {
            return name.Split(',', System.StringSplitOptions.RemoveEmptyEntries);
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