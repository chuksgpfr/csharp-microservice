using System;
using System.ComponentModel.DataAnnotations;

namespace CreateService.ViewModel
{
    public class LoginViewModel
    {
        [DataType(DataType.Text), Required]
        public string Email { get; set; }

        [DataType(DataType.Password), Required]
        public string Password { get; set; }
    }
}
