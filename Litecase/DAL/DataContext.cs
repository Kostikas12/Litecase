using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using Litecase.Models;

namespace Litecase.DAL
{
    public class DataContext : DbContext
    {
        public DataContext()
            :base("litecase")
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Sessions> Sessions { get; set; }
    }
}