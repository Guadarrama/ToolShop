using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace toolShop.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        // [Required(ErrorMessage = "Username is required")]
        // [MinLength(2, ErrorMessage = "First must be at least 2 characters long")]
        // public string UserName { get; set; } 

        [Required(ErrorMessage = "First name is required")]
        [MinLength(2, ErrorMessage = "First must be at least 2 characters long")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters long")]
        public string LastName { get; set; }        

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters in length")]
        [RegularExpression("^.*(?=.{8,})(?=.*[\\d])(?=.*[\\W]).*$", ErrorMessage = "Password must have one character, one number and one special character")]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        //One to many: Users +------< Stock
        public List<Product> Stock { get; set; }

        //One to Many: Users +----< CartItems
        public List<UserCart> CartItems { get; set; }

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
    }
}