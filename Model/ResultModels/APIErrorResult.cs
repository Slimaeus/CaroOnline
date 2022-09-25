using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResultModels
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public string[] ValidationErrors { get; set; } = Array.Empty<string>();

        public ApiErrorResult()
        {
            Succeeded = false;
        }

        public ApiErrorResult(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public ApiErrorResult(string[] validationErrors)
        {
            Succeeded = false;
            ValidationErrors = validationErrors;
        }
    }
}
