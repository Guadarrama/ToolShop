using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace toolShop.Models
{
    public class UserPurchase
    {
        [Key]
        public int UserPurchaseId {get; set; }

        public int Quantity {get; set;}

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        //One-to-One: User +----+ UserPurchases
        public int UserId { get; set; }
        public User UserPurchased { get; set; }

        //One-to-One: UserPurchase >
        public int ProductId { get; set; }
        public Product ItemPurchased{get; set;}

    }
}