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
            var result = _accountService.EventService(null);

            Assert.Equal(ReturnType.NotFound,result.ReturnType);
        }

        [Fact]
        public void CreateAccountOrDeposit_CreateNewAccount_ReturnAccountCreated()
        {
            var eventReq = new EventReqDto
            {
                Type = "deposit",
                Amount = 10,
                Destination = "100"
            };

            var result = _accountService.EventService(eventReq);

            Assert.Equal(ReturnType.Created, result.ReturnType);
            Assert.Equal(eventReq.Destination, result.data.Destination.Id);
        }

        [Fact]
        public void CreateAccountOrDeposit_DepositToAccount_ReturnAccount()
        {
            var eventReq = new EventReqDto
            {
                Type = "deposit",
                Amount = 10,
                Destination = "100"
            };

            //create account
            var accountCreation = _accountService.EventService(eventReq);

            //deposit into account
            var deposit = _accountService.EventService(eventReq);

            Assert.Equal(ReturnType.Created, deposit.ReturnType);
            Assert.Equal(eventReq.Destination, deposit.data.Destination.Id);
            Assert.Equal(eventReq.Amount * 2, deposit.data.Destination.Balance);

        }
    }
}
