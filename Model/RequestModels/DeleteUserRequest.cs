using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.RequestModels
{
    public class DeleteUserRequest
    {
        //public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
