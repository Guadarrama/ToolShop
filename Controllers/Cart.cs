using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace toolShop.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ItemId { get; set; }
        public Product Product { get; set; }

        public int Amount {get;set;}
    }
}