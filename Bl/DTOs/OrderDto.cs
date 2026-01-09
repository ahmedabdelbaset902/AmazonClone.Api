using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.DTOs
{
    public class OrderDto : BaseEntityDto
    {
       
            public int OrderId { get; set; }
            public int UserId { get; set; }
            public DateTime CreatedAt { get; set; }
            public decimal TotalPrice { get; set; }
            public List<OrderItemDto> Items { get; set; } = new();
     }

    public class CreateOrderDto
    {
        public int UserId { get; set; }
    }
}


