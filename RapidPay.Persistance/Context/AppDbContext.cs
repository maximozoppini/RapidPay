using Microsoft.EntityFrameworkCore;
using RapidPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Entities.Context
{
    public class AppDbContext: DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AuthorizationLog> AuthorizationsLogs { get; set; }
        public DbSet<PaymentFee> PaymentFees { get; set; }
        public DbSet<CardChangeLog> CardChangeLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

    }
    
}
