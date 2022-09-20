using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.RequestModels
{
    public class UpdateUserRequest
    {
        public string? UserName { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string? UpdatePassword { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? InGameName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
    }
}
