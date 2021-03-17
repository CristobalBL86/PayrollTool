using Microsoft.EntityFrameworkCore;
using PayrollTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollTool.Context
{
    public class PayrollContext : DbContext
    {
        public PayrollContext(DbContextOptions<PayrollContext> options) : base(options) { }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Operation> Operation { get; set; }
        public DbSet<Price> Price { get; set; }
        public DbSet<TransactionLog> TransacionLog { get; set; }
        public DbSet<AssistanceLog> AssistanceLog { get; set; }
        public DbSet<PayrollRelease> PayrollRelease { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<AssistanceLog>()
                .HasIndex(i => new { i.Date, i.EmployeeId })
                .IsUnique(true);

            builder.Entity<TransactionLog>()
                .HasIndex(i => new { i.Date, i.OperationId, i.ProductId })
                .IsUnique(true);
        }
    }
}
