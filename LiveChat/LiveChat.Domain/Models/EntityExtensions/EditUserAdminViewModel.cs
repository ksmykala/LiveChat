using LiveChat.Domain.Models.EntityClasses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LiveChat.Domain.Models.EntityExtensions
{
    public class EditUserAdminViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "New nickname")]
        public string Nickname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Roles")]
        public IEnumerable<webpages_Roles> Roles { get; set; }

        public IEnumerable<webpages_Roles> RolesToAdd { get; set; }
    }
}

