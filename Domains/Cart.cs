using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains
{
    public class Cart : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
