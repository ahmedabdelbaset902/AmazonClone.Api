using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.DTOs
{
    public class ProductImageDto : BaseEntityDto
    {
        public string ImageUrl { get; set; } = string.Empty;

        public int ProductId { get; set; }
       
    }
}
