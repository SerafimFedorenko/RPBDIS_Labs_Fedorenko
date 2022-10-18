﻿using Microsoft.EntityFrameworkCore;
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
        public RecPointContext(DbContextOptions<RecPointContext> options) : base(options)
        {

        }
    }
}