using loanapp.data.Entities;
using loanapp.Shared.Enums;
using loanapp.Shared.Interfaces;
using LoanApp.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace loanapp.application.Commands.Loans
{
    public class CreateLoanApplication
    {
        public class Command: IRequest<Response>
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

        public class Response: IResponse
        {
            public int LoanApplicationId { get; set; }

            public HttpStatusCode StatusCode { get; set; }

            public string ErrorMessage { get; set; }

            public Response(int Id)
            {
                StatusCode = HttpStatusCode.OK;
                LoanApplicationId = Id;
            }

            public Response(HttpStatusCode statusCode, string errorMessage)
            {
                StatusCode = statusCode;
                ErrorMessage = errorMessage;
            }
        }

        public class Handler: IRequestHandler<Command, Response>
        {
            private readonly LoanAppContext _readWriteContext;

            public Handler(LoanAppContext readWriteContext)
            {
                _readWriteContext = readWriteContext;
            }

            public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
            {
                var today = DateTime.UtcNow.Date;

                var hasExistingApplication = await _readWriteContext.LoanApplications
                    .Where(la => la.ApplicantName == command.ApplicantName 
                    && la.LoanAmount == command.LoanAmount
                    && la.LoanTerm == command.LoanTerm
                    && la.ApplicationDate.Date == today).AnyAsync();

                if (hasExistingApplication)
                {
                    return new Response(HttpStatusCode.Conflict, "An application with the same details already exists for today.");
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

                return new Response(entity.Id);
            }
        }

    }
}
