using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loanapp.Shared.Interfaces
{
    public interface IPagedRequest
    {
        int Page { get; set; }
        int PageLength { get; set; }
    }
}
