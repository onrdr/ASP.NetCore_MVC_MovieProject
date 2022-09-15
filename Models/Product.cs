
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        [MaxLength(10)]
        public string ISBN { get; set; }

        [Display(Name = "Director")]
        public string Author { get; set; }

        [Display(Name = "List Price")]
        [Range(1, 10000)]
        public double ListPrice { get; set; }

        [Display(Name = "Price For 1 - 50")]
        [Range(1, 10000)]
        public double Price { get; set; }

        [Display(Name = "Price For 51 - 100")]
        [Range(1, 10000)]
        public double Price50 { get; set; }

        [Display(Name = "Price For 100+")]
        [Range(1, 10000)]
        public double Price100 { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }
    }
}
