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

        public DepositDto<object> EventService(EventReqDto eventReqDto)
        {
            if (eventReqDto == null)
                return new DepositDto<object> { ReturnType = Utilities.ReturnType.NotFound };

            switch (eventReqDto.Type)
            {
                case "deposit":
                    return CreateAccountOrDeposit(eventReqDto);
                default:
                    return new DepositDto<object> { ReturnType = Utilities.ReturnType.NotFound };
            }
        }

        private DepositDto<object> CreateAccountOrDeposit(EventReqDto eventReqDto)
        {
            if (eventReqDto == null)
                return new DepositDto<object> { ReturnType = Utilities.ReturnType.NotFound};

            if (string.IsNullOrWhiteSpace(eventReqDto.Destination))
                return new DepositDto<object> { ReturnType = Utilities.ReturnType.NotFound };

            var account = _repository.Get(eventReqDto.Destination);

            if (account == null)
            {
                //initialise account
                var _account = new Account
                {
                    Id = eventReqDto.Destination,
                    Amount = eventReqDto.Amount
                };

                _repository.Add(_account);

                return new DepositDto<object>
                {
                    ReturnType = Utilities.ReturnType.Created,
                    data = new Data
                    {
                        Destination = new AccountDto
                        {
                            Id = _account.Id,
                            Balance = _account.Amount
                        }
                    }
                };
            }
            else
            {
                account.Amount += eventReqDto.Amount;

                _repository.Update(account);

                return new DepositDto<object>
                {
                    ReturnType = Utilities.ReturnType.Created,
                    data = new Data
                    {
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
}
