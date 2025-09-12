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
    public class UpdateLoanStatus
    {
        public class Command : IRequest<Response>
        {
            [Required]
            public int Id { get; set; }

            public LoanStatus LoanStatus { get; set; }
        }

        public class Response : IResponse
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

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly LoanAppContext _readWriteContext;

            private readonly ILoanApplicationService _loanApplicationService;

            private readonly ILogger<Handler> _logger;

            public Handler(LoanAppContext readWriteContext, ILogger<Handler> logger, ILoanApplicationService loanApplicationService)
            {
                _readWriteContext = readWriteContext;
                _logger = logger;
                _loanApplicationService = loanApplicationService;
            }

            public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
            {
                try
                {
                    var today = DateTime.UtcNow.Date;

                    var existingApplication = await _readWriteContext.LoanApplications
                        .Where(la => la.Id == command.Id).FirstOrDefaultAsync();

                    if (existingApplication == null)
                    {
                        return new Response(HttpStatusCode.NotFound, "This Application was not Found.");
                    }

                    await _loanApplicationService.UpdateLoanStatusAsync(command.Id, command.LoanStatus);

                    return new Response(HttpStatusCode.NoContent, null);
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "Error while updating loan application for {Application Id}", command.Id);

                    return new Response(HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
                }
            }
        }

    }
}
