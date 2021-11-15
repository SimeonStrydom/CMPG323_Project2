using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ptoject2.Models;

namespace ptoject2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ptoject2.Models.Photo> Photo { get; set; }
        public DbSet<ptoject2.Models.MetaData> MetaData { get; set; }
        public DbSet<ptoject2.Models.Album> Album { get; set; }
        public DbSet<ptoject2.Models.Tag> Tags { get; set; }
    }
}
