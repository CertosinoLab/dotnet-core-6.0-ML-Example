using System.Dynamic;

namespace MLDotnetExample.ML
{
    public interface IMLOperations
    {
        public ExpandoObject Train();
        public string Predict();
    }
}
