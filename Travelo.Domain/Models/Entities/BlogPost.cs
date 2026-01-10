using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class BlogPost:BaseEntity
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }

        public int Likes { get; set; }
        public int Views { get; set; }

        public IEnumerable<Comment> Comments { get; set; }


    }

    public class Comment:BaseEntity
    {
        public int BlogPostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}
