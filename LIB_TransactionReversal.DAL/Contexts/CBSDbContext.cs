using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;
using LIB_Usermanagement.DAL;
using Microsoft.EntityFrameworkCore;

namespace LIB_TransactionReversal.DAL.Contexts
{
    public class CBSDbContext : DbContext
    {
        public CBSDbContext(DbContextOptions<CBSDbContext> options) : base(options)
        {
        }


        public DbSet<CBSEthswichIncomingTransaction> CBSEthswichIncomingTransaction { get; set; }
        public DbSet<CBSPendingEthswichIncomingTransaction> CBSPendingEthswichIncomingTransaction { get; set; }
        public DbSet<CBSEthswichOutgoingTransaction> CBSEthswichOutgoingTransaction { get; set; }
        public DbSet<AccountBranch> AccountBranch { get; set; }
        public DbSet<TransactionValidation> TransactionValidation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CBSEthswichOutgoingTransaction>(entity =>
            {
                entity.Property(e => e.Id);
            });
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CBSEthswichOutgoingTransaction>().HasNoKey();
            modelBuilder.Entity<CBSEthswichIncomingTransaction>().HasNoKey();
            modelBuilder.Entity<CBSPendingEthswichIncomingTransaction>().HasNoKey();
            modelBuilder.Entity<AccountBranch>().HasNoKey();
            modelBuilder.Entity<TransactionValidation>().HasNoKey();
        }
    }
}
