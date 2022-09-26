using Model.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IJwtManager
    {
        public string Authenticate(AppUser user, IList<string> roles);
        public ClaimsPrincipal Validate(string token);
    }
}
