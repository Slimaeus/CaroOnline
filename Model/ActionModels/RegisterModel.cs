using Model.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ActionModels
{
    public class RegisterModel
    {
        public string ReturnUrl { get; set; } = string.Empty;
        public RegisterRequest Input { get; set; } = new RegisterRequest();
    }
}
