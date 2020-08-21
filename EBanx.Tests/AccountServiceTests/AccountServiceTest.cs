using System;
using EBANX.Core.Dtos;
using EBANX.Core.Interface;
using EBANX.Core.Services;
using EBANX.Core.Utilities;
using Moq;
using Xunit;

namespace EBanx.Tests.AccountServiceTests
{
    public class AccountServiceTest
    {
        private IAccountService _accountService { get; set; }
        private Mock<IRepository> _repository { get; set; }


        public AccountServiceTest()
        {
            _repository = new Mock<IRepository>();

            _accountService = new AccountService(_repository.Object);
        }

        [Fact]
        public void CreateAccount_InvalidRequestValid_ReturnNotFound()
        {
            var result = _accountService.CreateAccount(null);

            Assert.Equal(ReturnType.NotFound,result.ReturnType);
        }

        [Fact]
        public void CreateAccount_IsValidRequest_ReturnNotFound()
        {
            var eventReq = new EventReqDto
            {
                Amount = 10,
                Destination = "100"
            };

            var result = _accountService.CreateAccount(eventReq);

            Assert.Equal(ReturnType.Created, result.ReturnType);
            Assert.Equal(eventReq.Destination, result.data.Destination.Id);
        }
    }
}
