using ContactAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactAPI.Data
{
	public class ContactDbContext : DbContext
	{
        public ContactDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Contact> contacts  { get; set; }
    }
}
