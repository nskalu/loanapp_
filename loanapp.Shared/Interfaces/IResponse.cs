
using System.Net;

namespace loanapp.Shared.Interfaces
{
    public interface IResponse
    {
        HttpStatusCode StatusCode { get; set; }

        string ErrorMessage { get; set; }
    }
}
