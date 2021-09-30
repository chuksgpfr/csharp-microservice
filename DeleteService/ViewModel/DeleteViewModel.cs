using System;
using System.ComponentModel.DataAnnotations;

namespace DeleteService.ViewModel
{
    public class DeleteViewModel
    {
        [DataType(DataType.Text)]
        public string email { get; set; }
    }
}
