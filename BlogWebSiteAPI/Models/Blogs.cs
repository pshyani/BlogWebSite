using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWebSiteAPI.Models
{
    public partial class Blogs
    {
        public Blogs()
        {
            BlogComments = new HashSet<BlogComments>();
        }

        public Blogs(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }
        private ILazyLoader LazyLoader { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }
        
        public int UserId { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(1000)]
        public string Description { get; set; }
        public DateTime? Datecreated { get; set; }                
        //public Users User { get; set; }
        public ICollection<BlogComments> BlogComments { get; set; }
                
        private Users _user;
        public Users User
        {
            get => LazyLoader.Load(this, ref _user);
            set => _user = value;
        }
    }
}
