using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRomsMUtils
{
    public static class StringUtilities
    {
        public static string CapitalizeFirstLetter(string input)
        {
            switch (input)
            {
                // case null: throw new ArgumentNullException(nameof(input));
                // case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }

        public static string ReplaceHTMLEncoding(string text)
        {
            return System.Web.HttpUtility.HtmlDecode(text);
        }

        public static string AddCommasToNumber(int numberText)
        {
            return $"{numberText:n0}";
        }
    }
}
