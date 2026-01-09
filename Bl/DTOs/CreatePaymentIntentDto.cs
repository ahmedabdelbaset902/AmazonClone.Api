using System.ComponentModel.DataAnnotations;

namespace Bl.DTOs
{
    public class CreatePaymentIntentDto
    {
        [Required(ErrorMessage = "Amount is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; } = "usd";

        [Required]
        public int OrderId { get; set; }
    }
}
