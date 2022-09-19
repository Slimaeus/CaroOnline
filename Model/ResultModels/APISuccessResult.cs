using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResultModels
{
    public class APISuccessResult<T> : APIResult<T>
    {
        public APISuccessResult(T resultObject)
        {
            Succeeded = true;
            ResultObject = resultObject;
        }
        public APISuccessResult()
        {
            Succeeded = true;
        }
    }
}
