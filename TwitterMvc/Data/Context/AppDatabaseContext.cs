using System;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Dtos;
using TwitterMvc.Models;

namespace TwitterMvc.Data.Context
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options)
            : base(options)
        {
            
        }

        public DbSet<Post> Posts { get; set; }
    }
}
