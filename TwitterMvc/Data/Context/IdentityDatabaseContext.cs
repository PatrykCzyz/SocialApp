using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Models;

namespace TwitterMvc.Data.Context
{
    public class IdentityDatabaseContext : IdentityDbContext<CustomUser>
    {
        public IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options)
            : base(options)
        {

        }

        public DbSet<CustomUser> CustomUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
