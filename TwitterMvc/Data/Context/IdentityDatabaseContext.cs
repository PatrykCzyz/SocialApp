using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TwitterMvc.Data.Context
{
    public class IdentityDatabaseContext : IdentityDbContext
    {
        public IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options)
            : base(options)
        {
        }
    }
}
