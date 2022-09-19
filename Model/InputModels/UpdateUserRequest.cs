using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.InputModels
{
    public class UpdateUserRequest
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string InGameName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
