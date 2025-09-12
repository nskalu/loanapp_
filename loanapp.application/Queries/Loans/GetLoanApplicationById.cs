using loanapp.Shared.Enums;
using LoanApp.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Serialization;

namespace loanapp.application.Queries.Loans
{
    public class GetLoanApplicationById
    {
        public class Query : IRequest<Response>
        {
            [Required]
            public int Id { get; set; }
        }

        public class Response
        {
            public int LoanApplicationId { get; set; }

            public string ApplicantName { get; set; }

            public decimal LoanAmount { get; set; }

            public int LoanTerm { get; set; }

            public decimal InterestRate { get; set; }

            public LoanStatus LoanStatus { get; set; }

            public DateTime ApplicationDate { get; set; }

            [JsonIgnore]
            public HttpStatusCode StatusCode { get; set; }

            public string ErrorMessage { get; set; }

            public Response(HttpStatusCode statusCode, string errorMessage)
            {
                StatusCode = statusCode;
                ErrorMessage = errorMessage;
            }

            public Response()
            {
                StatusCode = HttpStatusCode.OK;
            }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly LoanAppContext _readWriteContext;

            private readonly ILogger<Handler> _logger;

            public Handler(LoanAppContext readWriteContext, ILogger<Handler> logger)
            {
                _readWriteContext = readWriteContext;
                _logger = logger;
            }

            public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
            {
                try
                {
                    var existingApplication = await _readWriteContext.LoanApplications
                        .Where(la => la.Id == query.Id).FirstOrDefaultAsync();

                    if (existingApplication == null)
                    {
                        return null;
                    }

                    var response = new Response 
                    { 
                        ApplicantName = existingApplication.ApplicantName,
                        LoanAmount = existingApplication.LoanAmount,
                        LoanTerm = existingApplication.LoanTerm,
                        InterestRate = existingApplication.InterestRate,
                        LoanStatus = existingApplication.LoanStatus,
                        ApplicationDate = existingApplication.ApplicationDate
                    };

                    return response;
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "Error while fetching loan application for {ApplicantId}", query.Id);

                    return new Response(HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
                }
            }
        }

    }
}
