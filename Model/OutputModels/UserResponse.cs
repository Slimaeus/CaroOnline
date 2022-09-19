using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.OutputModels
{
    public class UserResponse
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string InGameName { get; set; } = string.Empty;
        public ICollection<string> Roles { get; set; } = new List<string>();
    }
}
