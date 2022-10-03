using Model.RequestModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ActionModels
{
    public class LoginModel
    {
        public string ReturnUrl { get; set; } = string.Empty;
        public LoginRequest Input { get; set; } = new LoginRequest();
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; } = false;
    }
}
