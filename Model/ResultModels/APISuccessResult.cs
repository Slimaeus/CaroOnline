using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResultModels
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(T resultObject)
        {
            Succeeded = true;
            ResultObject = resultObject;
        }
        public ApiSuccessResult()
        {
            Succeeded = true;
        }
    }
}
