using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebSite.Models
{
    public partial class Blogs
    {
        public Blogs()
        {
            BlogComments = new HashSet<BlogComments>();
        }

        public int BlogId { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime? Datecreated { get; set; }

        public Users User { get; set; }
        public ICollection<BlogComments> BlogComments { get; set; }
    }
}
