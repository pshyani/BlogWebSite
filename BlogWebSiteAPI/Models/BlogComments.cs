using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWebSiteAPI.Models
{
    public partial class BlogComments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogCommentsId { get; set; }
        public int UserId { get; set; }
        public int BlogId { get; set; }
        [Required]
        public string Comment { get; set; }
        public DateTime? Datecreated { get; set; }

        public Blogs Blog { get; set; }
        public Users User { get; set; }
    }
}
