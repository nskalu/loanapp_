using loanapp.application.Interfaces;
using loanapp.data.Entities;
using loanapp.Shared.Enums;
using loanapp.Shared.Interfaces;
using LoanApp.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace loanapp.application.Commands.Loans
{
    public class CreateLoanApplication
    {
        public class Command: IRequest<Response>
        {

            [Required(ErrorMessage = "Applicant Name is required")]
            public string ApplicantName { get; set; }

            [Required(ErrorMessage = "Loan Amount is required")]
            [Range(0.01, double.MaxValue, ErrorMessage = "Loan Amount must be greater than 0")]

            public decimal? LoanAmount { get; set; }

            [Required(ErrorMessage = "Loan Term is required")]
            [Range(1, 12, ErrorMessage = "Loan Term must be between 1 and 12 months")]
            public int? LoanTerm { get; set; }

            [Required(ErrorMessage = "Interest Rate is required")]
            [Range(0, double.MaxValue, ErrorMessage = "Interest Rate cannot be negative")]
            public decimal? InterestRate { get; set; }
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
            private readonly ILoanApplicationService _loanApplicationService;

            private readonly ILogger<Handler> _logger;

            public Handler(ILoanApplicationService loanApplicationService, ILogger<Handler> logger)
            {
                _loanApplicationService = loanApplicationService;
                _logger = logger;
            }

            public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
            {
                try
                {
                   return await _loanApplicationService.CreateLoanApplicationAsync
                        (command.ApplicantName, command.LoanAmount.GetValueOrDefault(),
                        command.LoanTerm.GetValueOrDefault(), command.InterestRate.GetValueOrDefault());
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "Error while creating loan application for {Applicant}", command.ApplicantName);

                    return new Response(HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
                }
            }
        }

    }
}
