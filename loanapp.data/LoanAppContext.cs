using loanapp.data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace LoanApp.Data
{
    public class LoanAppContext : DbContext
    {
        public LoanAppContext(DbContextOptions<LoanAppContext> options)
            : base(options)
        {
        }
        public DbSet<LoanApplication> LoanApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LoanApplication>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ApplicantName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.LoanAmount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.LoanTerm)
                      .IsRequired();

                entity.Property(e => e.InterestRate)
                      .HasColumnType("decimal(5,2)");

                entity.Property(e => e.LoanStatus)
                      .IsRequired();

                entity.Property(e => e.ApplicationDate)
                      .HasColumnType("datetime")
                      .IsRequired();
            });
        }
    }
}
