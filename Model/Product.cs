using System.ComponentModel.DataAnnotations;

namespace FHSWebServiceApplication.Model
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
