using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace LoanApp.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<LoanAppContext>
    {
        public LoanAppContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LoanAppContext>();

            optionsBuilder.UseMySql(
                "server=localhost;database=LoanAppDb;user=root;password=123456",
                new MySqlServerVersion(new Version(8, 0, 36)) 
            );

            return new LoanAppContext(optionsBuilder.Options);
        }
    }
}
