using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace toolShop.Models
{
    public class Product
    {
        [Key]
        public int ProductId{get;set;}

        [Required]
        public string Name{get;set;}

        [Required]
        public string Description{get;set;}

        [Required]
        [Range(0.00, 1000000000.00)]
        public double Price{get;set;}

        [Required]
        [Range(1, 10000000000)]
        public int Quantity{get;set;}

        [Range(0, 10000000000)]
        public int AmountSold{get;set;} = 0;

        public bool isAvailable{get; set;} = true;

        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdatedAt{get;set;} = DateTime.Now;

        //One-to-Many: User +----< Product
        public int UserId { get; set; }
        public User Seller { get; set; }

        //One-to-Many: Product +----< UserCart
        public List<UserCart> Carts{get;set;}

    }
}