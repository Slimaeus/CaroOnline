using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.RequestModels
{
    public class RoleAssignRequest
    {
        public ICollection<SelectedItem> Roles { get; set; } = new List<SelectedItem>();
    }
}
