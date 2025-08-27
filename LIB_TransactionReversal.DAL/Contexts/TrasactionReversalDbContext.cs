using LIB_Documentmanagement.DAL.Entity;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;
using LIB_Usermanagement.DAL.Entity;
using LIB_Usermanagement.DAL.Entity.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LIB_Usermanagement.DAL
{
    public class TrasactionReversalDbContext:  DbContext
    {
        public TrasactionReversalDbContext(DbContextOptions<TrasactionReversalDbContext> options) :base(options)
        {
        }
        public DbSet<TransactionReversal> TransactionReversal { get; set; }
        public DbSet<TransactionAdjustement> TransactionAdjustement { get; set; }
        public DbSet<EthswitchOutgoingTransactionImport> EthswitchOutgoingTransactionImport { get; set; }
        public DbSet<TransactionNotFoundAtEthSwitch> TransactionNotFoundAtEthSwitch { get; set; }
        public DbSet<TransactionNotFoundAtLIB> TransactionNotFoundAtLIB { get; set; }
        public DbSet<SuccessfullTransaction> SuccessfullTransasction { get; set; }
        public DbSet<LibOutgoingTransaction> LibOutgoingTransaction { get; set; }
        public DbSet<LibIncommingTransaction> LibIncommingTransaction { get; set; }
        public DbSet<EthswitchIncommingTransactionImport> EthswitchIncommingTransactionImport { get; set; }
        public DbSet<EthswitchInvalidDateTransaction> EthswitchInvalidDateTransaction { get; set; }
        public DbSet<CBSEthswichIncomingTransaction> CBSEthswichIncomingTransaction { get; set; }
        public DbSet<CBSPendingEthswichIncomingTransaction> CBSPendingEthswichIncomingTransaction { get; set; }
        public DbSet<CBSEthswichOutgoingTransaction> CBSEthswichOutgoingTransaction { get; set; }
      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CBSEthswichOutgoingTransaction>(entity =>
            //{
            //    // Adding new columns
            //    entity.Property("Id")
            //          .HasColumnType("int")
            //          .HasAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
            //          .IsRequired(true); 
            //});

            //modelBuilder.Entity<CBSEthswichIncomingTransaction>(entity =>
            //{
            //    // Adding new columns
            //    entity.Property("Id")
            //          .HasColumnType("int")
            //          .HasAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
            //          .IsRequired(true); 
            //});
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

    }
}
