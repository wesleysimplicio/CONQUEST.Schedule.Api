using System.Text.RegularExpressions;

namespace CONQUEST.Schedule.Api
{
    public class Validation
    {
        public static bool IsValidEmail(string email)
        {
            bool isValid = false;
            string emailRegex = string.Format("{0}{1}",
                @"^(?("")("".+?(?<!\\)""@)|(([0-9A-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9A-z])@))",
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9A-z][-\w]*[0-9A-z]*\.)+[A-z0-9][\-A-z0-9]{0,22}[A-z0-9]))$");

            try
            {
                isValid = Regex.IsMatch(email, emailRegex);
            }
            catch (RegexMatchTimeoutException)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
