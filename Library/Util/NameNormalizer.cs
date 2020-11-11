using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryNet2020.Util
{
    public class NameNormalizer
	{
        const string ExcessCommasErrorMessage = "name can have at most one comma";
        
        public string Normalize(string unnormalizedName)
        {
            ThrowOnExcessCommas(unnormalizedName);
            var (baseName, suffix) = SplitOffSuffix(unnormalizedName);
            return FormatName(Parts(baseName.Trim()), suffix.Trim());
        }

        private string FormatName(string[] parts, string suffix)
        {
            return FormatBaseName(parts) + suffix;
        }

        private string FormatBaseName(string[] parts)
        {
            if (IsMononym(parts))
                return parts.First();
            if (IsDuonym(parts))
                return Last(parts) + ", " + First(parts);
            return Last(parts) + ", " + First(parts) + " " + MiddleInitials(parts);
        }

        private (string, string) SplitOffSuffix(string name)
        {
            var parts = name.Split(",");
            return parts.Length == 1 ? (parts[0], "") : (parts[0], $",{parts[1]}");
        }

        private void ThrowOnExcessCommas(string name)
        {
            if (name.Count(c => c == ',') > 1)
                throw new ArgumentException(ExcessCommasErrorMessage);
        }

        private string MiddleInitials(string[] parts)
        {
            return string.Join(" ", MiddleNames(parts).Select(Initial));
        }

        private IEnumerable<string> MiddleNames(string[] parts)
        {
            return parts[1..^1];
        }

        private string Initial(string name)
        {
            return name.Length == 1 ? name : $"{name[0]}.";
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
    }
}