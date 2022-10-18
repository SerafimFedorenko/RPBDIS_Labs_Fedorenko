using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecyclingPointLib.Models;

namespace RecyclingPointLib.Data
{
    public class RecPointContext : DbContext
    {
        public DbSet<Position>? Positions { get; set; }
        public DbSet<RecyclableType>? RecyclableTypes { get; set; }
        public DbSet<StorageType>? StorageTypes { get; set; }
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<Storage>? Storages { get; set; }
        public DbSet<AcceptedRecyclable>? AcceptedRecyclables { get; set; }
<<<<<<< Updated upstream
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationBuilder builder = new();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            IConfigurationRoot config = builder.Build();
            // получаем строку подключения
            //string connectionString = config.GetConnectionString("SqliteConnection");
            string connectionString = config.GetConnectionString("SQLConnection");
            _ = optionsBuilder
                .UseSqlServer(connectionString)
                //.UseSqlite(connectionString)
                .Options;
            optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
=======
        public RecPointContext(DbContextOptions<RecPointContext> options) : base(options)
        {

>>>>>>> Stashed changes
        }
    }
}