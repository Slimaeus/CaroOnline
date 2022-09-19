using Model.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IJWTManager
    {
        public string Authenticate(User user, IList<string> roles);
    }
}
