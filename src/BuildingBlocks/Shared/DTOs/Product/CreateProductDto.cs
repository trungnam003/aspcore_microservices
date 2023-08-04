#nullable disable

namespace Shared.DTOs.Product
{
    public class CreateProductDto
    {
        public string No { get; set; }

        public string Name { get; set; }

        public string Sumary { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
