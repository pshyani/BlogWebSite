using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebSite.Models
{
    public partial class Users
    {
        public Users()
        {
            BlogComments = new HashSet<BlogComments>();
            Blogs = new HashSet<Blogs>();
        }

        public int UserId { get; set; }
        [Required]
        [MaxLength(20), MinLength(2)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(20), MinLength(2)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }
        public ICollection<BlogComments> BlogComments { get; set; }
        public ICollection<Blogs> Blogs { get; set; }
    }
}
