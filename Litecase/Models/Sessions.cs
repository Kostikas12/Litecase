using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Litecase.Models
{
    public class Sessions
    {
        [Key]
        public int ID_Session { get; set; }
        public int ID_User { get; set; }
        public DateTime TimeStart { get; set; }

        public Sessions() { }
        public Sessions(int ID_User, DateTime TimeStart)
        {
            this.ID_User = ID_User;
            this.TimeStart = TimeStart;
        }
    }
}