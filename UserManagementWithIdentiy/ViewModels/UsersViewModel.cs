using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserManagementWithIdentiy.ViewModels
{
    public class UsersViewModel
    {
        public string id { get; set; }
        public byte[] ProfilePicture { get; set; }
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Display(Name ="Last Name")]
        public string LastName { get; set; }
        [Display(Name ="User Name")]
        public string UserName { get; set; }
        [Display(Name ="Email")]
        public string Email { get; set; }
        [Display(Name ="Role")]
        public IEnumerable<string> RoleName { get; set; }

    }
}
