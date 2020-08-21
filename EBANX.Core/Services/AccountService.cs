using EBANX.Core.Dtos;
using EBANX.Core.Interface;
using EBANX.Core.Models;

namespace EBANX.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository _repository;

        public AccountService(IRepository repository)
        {
            _repository = repository;
        }

        public DepositDto<object> CreateAccount(EventReqDto eventReqDto)
        {
            if (eventReqDto == null)
                return new DepositDto<object> { ReturnType = Utilities.ReturnType.NotFound};

            var account = new Account
            {
                Id = eventReqDto.Destination,
                Amount = eventReqDto.Amount
            };

            _repository.Add(account);

            return new DepositDto<object>
            {
                ReturnType = Utilities.ReturnType.Created,
                data = new Data {
                     Destination = new AccountDto
                     {
                         Id = account.Id,
                         Balance = account.Amount
                     }
                }
            };
        }
        
    }
}
