using loanapp.application.Commands.Loans;
using loanapp.application.Queries.Loans;
using loanapp.data.Entities;
using loanapp.Shared.Enums;
using LoanApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static loanapp.application.Commands.Loans.CreateLoanApplication;

namespace loanapp.application.Interfaces
{
    public interface ILoanApplicationService
    {
        Task<CreateLoanApplication.Response> CreateLoanApplicationAsync(string applicantName, decimal loanAmount, int loanTerm, decimal interestRate);
        Task<UpdateLoanApplication.Response> UpdateLoanApplicationAsync(int id, string applicantName, decimal loanAmount, int loanTerm, decimal interestRate, LoanStatus loanStatus);
        Task<GetLoanApplicationById.Response> GetLoanApplicationByIdAsync(int id);
        Task<GetLoanApplications.Result> GetLoanApplicationsAsync(int pageNumber, int pageLength);
        Task<DeleteLoanApplication.Response> DeleteLoanApplication(int Id);
        Task<int> UpdateLoanStatusAsync(int id, LoanStatus loanStatus);


    }

    public class LoanApplicationService : ILoanApplicationService
    {
        private readonly LoanAppContext _readWriteContext;

        public LoanApplicationService(LoanAppContext readWriteContext)
        { 
            _readWriteContext = readWriteContext;
        }
        public async Task<CreateLoanApplication.Response> CreateLoanApplicationAsync(string applicantName, decimal loanAmount, int loanTerm, decimal interestRate)
        {

            var entity = new LoanApplication
            {
                ApplicantName = applicantName,
                LoanAmount = loanAmount,
                LoanTerm = loanTerm,
                InterestRate = interestRate,
                LoanStatus = LoanStatus.Pending,
                ApplicationDate = DateTime.UtcNow
            };

            _readWriteContext.LoanApplications.Add(entity);
            await _readWriteContext.SaveChangesAsync();

            return new Response(entity.Id);
        }
        public Task<UpdateLoanApplication.Response> UpdateLoanApplicationAsync(int id, string applicantName, decimal loanAmount, int loanTerm, decimal interestRate, LoanStatus loanStatus)
        {
            throw new NotImplementedException();
        }
        public Task<GetLoanApplicationById.Response> GetLoanApplicationByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<GetLoanApplications.Result> GetLoanApplicationsAsync(int pageNumber, int pageLength)
        {
            throw new NotImplementedException();
        }
        public Task<DeleteLoanApplication.Response> DeleteLoanApplication(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateLoanStatusAsync(int id, LoanStatus loanStatus)
        {
            var today = DateTime.UtcNow.Date;

            var existingApplication = await _readWriteContext.LoanApplications
                .SingleAsync(la => la.Id == id);

            existingApplication.LoanStatus = loanStatus;

            return await _readWriteContext.SaveChangesAsync();
        }
    }
}
