using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebApplication.Models
{
    public class User
    {
        public Guid ID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Permissions UserPermissions {  get; set; } //0:User 1:Manager
    }

    public class UserDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}

public enum Permissions
{
    User = 0,
    Manager = 1
}