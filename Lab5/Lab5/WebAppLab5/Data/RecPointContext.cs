using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebAppLab5.Models;

namespace WebAppLab5.Data
{
    public class RecPointContext : DbContext
    {
        public DbSet<Position>? Positions { get; set; }
        public DbSet<RecyclableType>? RecyclableTypes { get; set; }
        public DbSet<StorageType>? StorageTypes { get; set; }
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<Storage>? Storages { get; set; }
        public DbSet<AcceptedRecyclable>? AcceptedRecyclables { get; set; }
        public RecPointContext(DbContextOptions<RecPointContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    ConfigurationBuilder builder = new();
        //    string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=recycling_point0;Integrated Security=True";
        //    _ = optionsBuilder
        //        .UseSqlServer(connectionString)
        //        .Options;
        //    optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
        //}
    }
}