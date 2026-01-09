using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains
{
    public class Review : BaseEntity
    {
        public int Rating { get; set; }  // 1 → 5
        public string Comment { get; set; } = string.Empty;

        public int UserId { get; set; }
        public int ProductId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
