using System;
using System.ComponentModel.DataAnnotations;

namespace CreateService.ViewModel
{
    public class SignupViewModel
    {
        [DataType(DataType.Text), Required(ErrorMessage = "First name is required"), MinLength(4, ErrorMessage = "First name is a minimum of 4 characters ")]
        public string Firstname { get; set; }

        [DataType(DataType.EmailAddress), Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [DataType(DataType.Text), Required(ErrorMessage = "Last name is required"), MinLength(4)]
        public string Lastname { get; set; }

        [DataType(DataType.PhoneNumber), Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Text), Required]
        public string Country { get; set; }

        [DataType(DataType.Text), Required]
        public string State { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Password is required"), MinLength(8, ErrorMessage = "Password is a minimum of 8 characters ")]
        public string Password { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Confirm Password is required"), Compare("Password", ErrorMessage = "Password does not match")]
        public string ConfrimPassword { get; set; }
    }
}
