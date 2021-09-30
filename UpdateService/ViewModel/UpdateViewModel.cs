using System;
using System.ComponentModel.DataAnnotations;

namespace UpdateService.ViewModel
{
    public class UpdateViewModel
    {
        [DataType(DataType.Text), Required(ErrorMessage = "Full name is required"), MinLength(4, ErrorMessage = "First name is a minimum of 4 characters ")]
        public string Firstname { get; set; }

        [DataType(DataType.EmailAddress), Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [DataType(DataType.Text), Required, MinLength(4)]
        public string Lastname { get; set; }

        [DataType(DataType.PhoneNumber), Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Text), Required]
        public string Country { get; set; }

        [DataType(DataType.Text), Required]
        public string State { get; set; }
    }
}
