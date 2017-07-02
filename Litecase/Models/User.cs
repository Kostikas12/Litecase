using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Litecase.Models
{
    public class User
    {
        [Key]
        public int ID_User { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }

        public User()
        { }
        public User(string Name, string Password, string Mail)
        {
            this.Name = Name;
            this.Password = Password;
            this.Mail = Mail;
        }
    }
}