using System;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Dtos;

namespace TwitterMvc.Data.Context
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<IdentityDatabaseContext> options)
            : base(options)
        {

        }
    }
}
