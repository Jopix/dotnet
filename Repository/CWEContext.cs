using Ninesky.Models;
using System.Data.Entity;

namespace Ninesky.Repository
{
    public class CWEContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public CWEContext()
            : base("DefaultConnection")
        {
            Database.CreateIfNotExists();
        }
    }
}