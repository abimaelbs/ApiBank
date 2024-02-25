using ApiBank.Web.Database.Map;
using ApiBank.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiBank.Web.Database
{
    public class ApiBankContext : DbContext
    {
        public ApiBankContext(DbContextOptions<ApiBankContext> options) : base(options) { }
        
        public DbSet<Conta> Contas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContaMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
