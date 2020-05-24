using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Models;

namespace TwitterMvc.Data.Context
{
    public class AppDbContext : IdentityDbContext<CustomUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<CustomUser> CustomUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
