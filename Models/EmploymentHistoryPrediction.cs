using Microsoft.ML.Data;

namespace MLDotnetExample.Models
{
    public class EmploymentHistoryPrediction
    {
        [ColumnName("Score")]
        public float DurationInMonths;
    }
}
