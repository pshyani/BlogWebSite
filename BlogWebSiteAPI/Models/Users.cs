using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWebSiteAPI.Models
{
    public partial class Users
    {
        public Users()
        {
            BlogComments = new HashSet<BlogComments>();
            Blogs = new HashSet<Blogs>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }

        public ICollection<BlogComments> BlogComments { get; set; }
        public ICollection<Blogs> Blogs { get; set; }
    }
}
