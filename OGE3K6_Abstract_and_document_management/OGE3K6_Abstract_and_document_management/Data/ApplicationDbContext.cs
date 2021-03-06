using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OGE3K6_Abstract_and_document_management.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OGE3K6_Abstract_and_document_management.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Abstract> Abstracts { get; set; }
        public DbSet<SiteUser> Users { get; set; }
    }
}
