using loanapp.Shared.Enums;

namespace loanapp.data.Entities
{
    public class LoanApplication
    {
        public int Id { get; set; }

        public string ApplicantName { get; set; }

        public decimal LoanAmount { get; set; }

        public int LoanTerm { get; set; }

        public decimal InterestRate { get; set; }

        public LoanStatus LoanStatus { get; set; }

        public DateTime ApplicationDate { get; set; }
    }
}
