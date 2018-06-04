using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public class EmailValidator : ValidationRule
    {
        Regex whitespace = new Regex(@"\s");
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var emailValidatorClass = new RegexUtilities();
            App app = (App)Application.Current;
            string message = (string)app.ThemeDictionary["invalidEmailError"];
            if (value == null)
                return new ValidationResult(false, message);
            else
            {
                if (!emailValidatorClass.IsValidEmail(value.ToString()) || whitespace.IsMatch(value.ToString()))
                    return new ValidationResult(false, message);
            }
            return ValidationResult.ValidResult;
        }
    }

    public class TitleValidator : ValidationRule
    {
        Regex allWhitespaces = new Regex(@"^\s+$");
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            App app = (App)Application.Current;
            string message = (string)app.ThemeDictionary["invalidTitleError"];
            if (value == null)
                return new ValidationResult(false, message);
            else
            {
                if (value.ToString().Length == 0 || allWhitespaces.IsMatch(value.ToString()))
                    return new ValidationResult(false, message);
            }
            return ValidationResult.ValidResult;
        }
    }

    public class MessageValidator : ValidationRule
    {
        Regex allWhitespaces = new Regex(@"^\s+$");
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            App app = (App)Application.Current;
            string message = (string)app.ThemeDictionary["messageToShortError"];
            if (value == null)
                return new ValidationResult(false, message);
            else
            {
                if (value.ToString().Length < 10 || allWhitespaces.IsMatch(value.ToString()))
                    return new ValidationResult(false, message);
            }
            return ValidationResult.ValidResult;
        }
    }

    public class RegexUtilities
    {
        bool invalid = false;

        public bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid email format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}
