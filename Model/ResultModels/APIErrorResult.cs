using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResultModels
{
    public class APIErrorResult<T> : APIResult<T>
    {
        public string[] ValidationErrors { get; set; } = Array.Empty<string>();

        public APIErrorResult()
        {
            Succeeded = false;
        }

        public APIErrorResult(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public APIErrorResult(string[] validationErrors)
        {
            Succeeded = false;
            ValidationErrors = validationErrors;
        }
    }
}
