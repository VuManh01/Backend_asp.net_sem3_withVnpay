using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace project3api_be.Dtos
{
    public class RegisterDto
    {   
        
        public required string Email { get; set; }
       
        public required string Password { get; set; }
     
        public required string FullName { get; set; }
    }
}