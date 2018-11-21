using System.Text.RegularExpressions;

namespace OctopusCodesMultiVendor.Helpers
{
    public class PasswordHelper
    {
        public static bool IsValidPassword(string password)
        {
            Regex regex = new Regex("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,20})");
            Match match = regex.Match(password);
            return match.Success;
        }
    }
}