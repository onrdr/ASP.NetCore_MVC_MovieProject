﻿
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class ShoppingCart
    {

        public int Id { get; set; }


        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        public int ProductId { get; set; }


        [Range(1, 1000, ErrorMessage = "The value must be between 1 and 1000")]
        public int Count { get; set; }


        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
