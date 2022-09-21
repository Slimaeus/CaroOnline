using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResponseModels
{
    public class UserResponse
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string InGameName { get; set; } = string.Empty;
        public int Level { get; set; } = 1;
        public int Exp { get; set; } = 0;
        public int Score { get; set; } = 0;
        public ICollection<string> Roles { get; set; } = new List<string>();
    }
}
