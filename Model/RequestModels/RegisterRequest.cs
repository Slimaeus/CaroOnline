using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Model.RequestModels
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; } = string.Empty;
        [Required]
        [Display(Name = "InGame Name")]
        public string InGameName { get; set; } = string.Empty;
    }
}
