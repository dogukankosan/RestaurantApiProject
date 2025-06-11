using System.Text.RegularExpressions;

namespace RestaurantWebUI.HelpersMethod
{
    internal static class PhoneFormatter
    {
        internal static string ToPrettyPhone(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return raw;
            string digits = Regex.Replace(raw, @"[^\d]", "");
            if (digits.Length == 12 && digits.StartsWith("90"))
            {
                string country = digits.Substring(0, 2);
                string part1 = digits.Substring(2, 3);
                string part2 = digits.Substring(5, 3);
                string part3 = digits.Substring(8, 4);
                return $"(+{country}) {part1} {part2} {part3}";
            }
            return raw;
        }
    }
}