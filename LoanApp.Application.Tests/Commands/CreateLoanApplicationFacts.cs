using loanapp.application.Commands.Loans;
using loanapp.application.Interfaces;
using LoanApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Xunit;

namespace loanapp.tests.Commands
{
    public class CreateLoanApplicationFacts
    {
        private readonly LoanAppContext _dbContext;
        private readonly Mock<ILoanApplicationService> _loanAppServiceMock;
        private readonly Mock<ILogger<CreateLoanApplication.Handler>> _loggerMock;

        public CreateLoanApplicationFacts()
        {
            // Use EF InMemory DB
            var options = new DbContextOptionsBuilder<LoanAppContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new LoanAppContext(options);
            _loanAppServiceMock = new Mock<ILoanApplicationService>();
            _loggerMock = new Mock<ILogger<CreateLoanApplication.Handler>>();
        }

        [Fact]
        public async Task Should_Return_Ok_When_Application_Is_Created()
        {
            // Arrange
            var command = new CreateLoanApplication.Command
            {
                ApplicantName = "Ngozi Kalu",
                LoanAmount = 1000,
                LoanTerm = 6,
                InterestRate = 5
            };

            // Mock service call
            _loanAppServiceMock.Setup(s => s.CreateLoanApplicationAsync(
                    command.ApplicantName, command.LoanAmount.Value, command.LoanTerm.Value, command.InterestRate.Value))
                .ReturnsAsync(new CreateLoanApplication.Response(1));

            var handler = new CreateLoanApplication.Handler(
                _dbContext, _loanAppServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(1, result.LoanApplicationId);
        }

        [Fact]
        public async Task Should_Return_Conflict_When_Duplicate_Exists()
        {
            // Arrange
            var today = DateTime.UtcNow.Date;
            _dbContext.LoanApplications.Add(new loanapp.data.Entities.LoanApplication
            {
                ApplicantName = "Ngozi Kalu",
                LoanAmount = 2000,
                LoanTerm = 12,
                InterestRate = 10,
                ApplicationDate = today
            });
            await _dbContext.SaveChangesAsync();

            var command = new CreateLoanApplication.Command
            {
                ApplicantName = "Ngozi Kalu",
                LoanAmount = 2000,
                LoanTerm = 12,
                InterestRate = 10
            };

            var handler = new CreateLoanApplication.Handler(
                _dbContext, _loanAppServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Equal("An application with the same details already exists for today.", result.ErrorMessage);
        }
    }
}
