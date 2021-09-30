using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DeleteService.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DataType(DataType.Text), Required, MinLength(4)]
        public string Firstname { get; set; }

        [DataType(DataType.Text), Required, MinLength(4)]
        public string Lastname { get; set; }

        //[DataType(DataType.PhoneNumber), Required]
        //public string Phone { get; set; }

        [DataType(DataType.Text), Required]
        public string Country { get; set; }

        [DataType(DataType.Text), Required]
        public string State { get; set; }

        [DataType(DataType.DateTime), Required]
        public DateTime DateRegistered { get; set; }
    }
}
