using Microsoft.ML;
using MLDotnetExample.Common;

namespace MLDotnetExample.ML.Base
{
    public class BaseML
    {
        protected static string ModelPath()
        {
            return Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\MLDotnetExample\\TrainedModels",
                    Constants.EMPLOYEE_MODEL);
        }

        protected readonly MLContext MlContext;

        protected BaseML()
        {
            MlContext = new MLContext(2023);
        }
    }
}
