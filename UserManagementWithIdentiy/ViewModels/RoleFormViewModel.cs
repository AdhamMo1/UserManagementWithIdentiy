using System.ComponentModel.DataAnnotations;

namespace UserManagementWithIdentiy.ViewModels
{
    public class RoleFormViewModel
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
    }
}
