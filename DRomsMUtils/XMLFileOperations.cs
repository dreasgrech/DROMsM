

namespace DRomsMUtils
{
    public static class XMLFileOperations
    {
        public static string NormalizeTextAndCapitalizeFirstLetter(string text)
        {
            var normalizedText = NormalizeText(text);
            normalizedText = StringUtilities.CapitalizeFirstLetter(normalizedText);

            return normalizedText;
        }

        public static string NormalizeText(string text)
        {
            return StringUtilities.ReplaceHTMLEncoding(text);
        }

    }
}