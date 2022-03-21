using System.Runtime.CompilerServices;

namespace Frontend
{
    public class OperationsOptions
    {
        public double AllowedSimilarityValue { get; set; }
        public bool MatchUsingGameListXMLName { get; set; }
        public bool AutoExpandAfterOperations { get; set; }

        public static OperationsOptions Instance { get; }

        static OperationsOptions()
        {
            Instance = new OperationsOptions();
        }
    }
}