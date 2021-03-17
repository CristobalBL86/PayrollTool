using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollTool.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required(ErrorMessage ="User name is required")]
        [Display(Name ="User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Password is required")]
        [MinLength(8,ErrorMessage ="Password must be at least 8 characters length")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
