using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.DTOs
{
    public class ReviewDto : BaseEntityDto
    {
        public int Rating { get; set; }  // 1 → 5
        public string Comment { get; set; } = string.Empty;

        public int UserId { get; set; }
        public int ProductId { get; set; }

    }
}
