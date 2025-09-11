using loanapp.data.Entities;
using loanapp.Shared.Enums;
using LoanApp.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace loanapp.application.Commands.Loans
{
    public class CreateLoanApplication
    {
        public class Command
        {
            [Required]
            public string ApplicantName { get; set; }

            [Required]
            public decimal LoanAmount { get; set; }

            [Required]
            public int LoanTerm { get; set; }

            [Required]
            public decimal InterestRate { get; set; }
        }

        public class Handler
        {
            private readonly LoanAppContext _readWriteContext;

            public Handler(LoanAppContext readWriteContext)
            {
                _readWriteContext = readWriteContext;
            }

            public async Task<int> Handle(Command command)
            {
                var today = DateTime.UtcNow.Date;

                var hasExistingApplication = await _readWriteContext.LoanApplications
                    .Where(la => la.ApplicantName == command.ApplicantName 
                    && la.LoanAmount == command.LoanAmount
                    && la.LoanTerm == command.LoanTerm
                    && la.ApplicationDate.Date == today).AnyAsync();

                if (hasExistingApplication)
                {
                    throw new InvalidOperationException("A loan application with the same details already exists for today.");
                }

                var entity = new LoanApplication
                {
                    ApplicantName = command.ApplicantName,
                    LoanAmount = command.LoanAmount,
                    LoanTerm = command.LoanTerm,
                    InterestRate = command.InterestRate,
                    LoanStatus = LoanStatus.Pending,
                    ApplicationDate = DateTime.UtcNow
                };

                _readWriteContext.LoanApplications.Add(entity);
                await _readWriteContext.SaveChangesAsync();

                return entity.Id;
            }
        }

    }
}
