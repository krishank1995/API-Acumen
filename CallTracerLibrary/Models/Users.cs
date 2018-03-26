using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CallTracerLibrary.Models
{
   public  class Users
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }
    }
}
