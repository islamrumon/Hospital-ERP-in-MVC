using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASITHmsMvc.Models.DtoModels
{
    public class LoginDto
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage ="Username is Required")]
        
        public string username { get; set; }
        [Required(ErrorMessage ="Password is Requried")]

        public string password { get; set; }

        //public LoginDto() {
        //    username.Trim();
        //    password.Trim();
        //}
    }
}