using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebSite.Models
{
    public partial class BlogComments
    {
        public int BlogCommentsId { get; set; }
        public int UserId { get; set; }
        public int BlogId { get; set; }
        public string Comment { get; set; }
        public DateTime? Datecreated { get; set; }

        public Blogs Blog { get; set; }
        public Users User { get; set; }
    }
}
