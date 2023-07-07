using Microsoft.AspNetCore.Mvc;
using MLDotnetExample.ML;
using System.Dynamic;

namespace MLDotnetExample.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeesAttrictionController : ControllerBase
    {
        IMLOperations _IMLoperations = new MLOperations();

        [HttpGet("/train")]
        public IActionResult Train()
        {
            ExpandoObject result = _IMLoperations.Train();
            return Ok(result);
        }

        [HttpGet("/predict")]
        public IActionResult Predict()
        {
            string predictorResult = _IMLoperations.Predict();

            dynamic result = new ExpandoObject();
            result.prediction = predictorResult;

            return Ok(result);
        }
    }
}
