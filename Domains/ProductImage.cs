using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains
{
    public class ProductImage : BaseEntity
    {
        public string ImageUrl { get; set; } = string.Empty;

        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
    }
}
