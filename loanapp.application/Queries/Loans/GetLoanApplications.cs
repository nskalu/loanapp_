using loanapp.Shared.Enums;
using loanapp.Shared.Helpers;
using loanapp.Shared.Interfaces;
using LoanApp.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Serialization;

namespace loanapp.application.Queries.Loans
{
    public class GetLoanApplications
    {
        public class Query : IRequest<Result>, IPagedRequest
        {
            [Required]
            public int Page { get; set; }

            [Required]
            public int PageLength { get; set; }
        }

        public class Result : PagedResult<Response>
        {
            [JsonIgnore]
            public HttpStatusCode StatusCode { get; internal set; }

            [JsonIgnore]
            public string ErrorMessage { get; internal set; }
        }

        public class Response
        {
            public int Id { get; set; }

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

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly LoanAppContext _readWriteContext;

            private readonly ILogger<Handler> _logger;

            public Handler(LoanAppContext readWriteContext, ILogger<Handler> logger)
            {
                _readWriteContext = readWriteContext;
                _logger = logger;
            }

            public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
            {
                try
                {
                    var existingApplications = await (from application in _readWriteContext.LoanApplications 
                    select new Response
                    {
                        Id = application.Id,
                        ApplicantName = application.ApplicantName,
                        LoanAmount = application.LoanAmount,
                        LoanTerm = application.LoanTerm,
                        InterestRate = application.InterestRate,
                        LoanStatus = application.LoanStatus,
                        ApplicationDate = application.ApplicationDate
                    }).GetSkipAndTake<Response, Result>(query);

                    if (existingApplications == null)
                    {
                        return null;
                    }

                    return existingApplications;
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "Error while fetching loan applications");
                    return new Result
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrorMessage = "An error occurred while processing your request."
                    };
                }
            }
        }

    }
}
