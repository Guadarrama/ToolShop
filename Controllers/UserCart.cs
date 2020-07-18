using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace toolShop.Models
{
    public class UserCart
    {
        [Key]
        public int UserCartId { get; set; }

        public int Amount { get; set; }

        //One-to-Many: Buyer +-----< UserCart
        public int UserId { get; set; }
        public User Buyer { get; set; }

        //One-to-Many: UserCart >-----+ Product
        public int ProductId{get;set;}
        public Product ItemInCart{get;set;}
    }
}