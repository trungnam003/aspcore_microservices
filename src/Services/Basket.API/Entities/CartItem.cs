#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Basket.API.Entities
{
    public class CartItem
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        [Range(0.1, double.PositiveInfinity)]
        public decimal ItemPrice { get; set; }

        [Required]
        public string ItemNo { get; set; }
        [Required]
        public string ItemtName { get; set; }

        public int AvailableQuantity { get; set; } 

        public void SetAvailableQuantity(int availableQuantity)
        {
            AvailableQuantity = availableQuantity;
        }
    }
}
