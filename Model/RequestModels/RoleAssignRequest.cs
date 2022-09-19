using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.RequestModels
{
    public class RoleAssignRequest
    {
        public Guid Id { get; set; } = new Guid();
        public ICollection<SelectedItem> Roles { get; set; } = new List<SelectedItem>();
    }
}
