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

        public TransactionDto EventService(EventReqDto eventReqDto)
        {
            if (eventReqDto == null)
                return new TransactionDto { ReturnType = Utilities.ReturnType.NotFound };

            switch (eventReqDto.Type)
            {
                case "deposit":
                    var deposit = CreateAccountOrDeposit(eventReqDto);
                    return new TransactionDto { ReturnType = deposit.ReturnType, Data = deposit.DepositData };
                case "withdraw":
                    var withdraw = Withrawal(eventReqDto);
                    return new TransactionDto { ReturnType = withdraw.ReturnType, Data = withdraw.WithdrawData };
                case "transfer":
                    var transfer = Transfer(eventReqDto);
                    return new TransactionDto
                    {
                        ReturnType = transfer.ReturnType,
                        Data = new
                        {
                            origin = transfer.WithdrawData?.Origin,
                            destination = transfer.DepositData?.Destination
                        }
                    };
                default:
                    return new TransactionDto { ReturnType = Utilities.ReturnType.NotFound };
            }
        }

        private DepositDto CreateAccountOrDeposit(EventReqDto eventReqDto)
        {
            if (eventReqDto == null)
                return new DepositDto { ReturnType = Utilities.ReturnType.NotFound };

            if (string.IsNullOrWhiteSpace(eventReqDto.Destination))
                return new DepositDto { ReturnType = Utilities.ReturnType.NotFound };

            var account = _repository.Get(eventReqDto.Destination);

            if (account == null)
            {
                //initialise account
                var _account = new Account
                {
                    Id = eventReqDto.Destination,
                    Balance = eventReqDto.Amount
                };

                _repository.Add(_account);

                return new DepositDto
                {
                    ReturnType = Utilities.ReturnType.Created,
                    DepositData = new Data
                    {
                        Destination = new AccountDto
                        {
                            Id = _account.Id,
                            Balance = _account.Balance
                        }
                    }
                };
            }
            else
            {
                account.Balance += eventReqDto.Amount;

                _repository.Update(account);

                return new DepositDto
                {
                    ReturnType = Utilities.ReturnType.Created,
                    DepositData = new Data
                    {
                        Destination = new AccountDto
                        {
                            Id = account.Id,
                            Balance = account.Balance
                        }
                    }
                };
            }
        }

        private WithdrwalDto Withrawal(EventReqDto eventReqDto)
        {
            if (eventReqDto == null)
                return new WithdrwalDto { ReturnType = Utilities.ReturnType.NotFound };

            if (string.IsNullOrWhiteSpace(eventReqDto.Origin))
                return new WithdrwalDto { ReturnType = Utilities.ReturnType.NotFound };

            var account = _repository.Get(eventReqDto.Origin);

            if (account == null)
            {
                return new WithdrwalDto
                {
                    ReturnType = Utilities.ReturnType.NotFound,
                    WithdrawData = null
                };
            }
            else
            {
                if (account.Balance <= 0)
                    return new WithdrwalDto { ReturnType = Utilities.ReturnType.NotFound };

                account.Balance -= eventReqDto.Amount;

                _repository.Update(account);

                return new WithdrwalDto
                {
                    ReturnType = Utilities.ReturnType.Created,
                    WithdrawData = new WithDrawData
                    {
                        Origin = new AccountDto
                        {
                            Id = account.Id,
                            Balance = account.Balance
                        }
                    }
                };
            }
        }

        public BalanceDto GetBalance(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId))
                return new BalanceDto { ReturnType = Utilities.ReturnType.NotFound, Balance = 0 };

            var account = _repository.Get(accountId);

            if (account == null)
                return new BalanceDto { ReturnType = Utilities.ReturnType.NotFound, Balance = 0 };


            return new BalanceDto { ReturnType = Utilities.ReturnType.Ok, Balance = account.Balance };
        }

        private TransferDto Transfer2(EventReqDto eventReqDto)
        {
            if (eventReqDto == null)
                return new TransferDto { ReturnType = Utilities.ReturnType.NotFound };

            if (string.IsNullOrWhiteSpace(eventReqDto.Origin) || string.IsNullOrWhiteSpace(eventReqDto.Destination))
                return new TransferDto { ReturnType = Utilities.ReturnType.NotFound };

            var originAccount = _repository.Get(eventReqDto.Origin);

            if (originAccount == null)
                return new TransferDto { ReturnType = Utilities.ReturnType.NotFound };

            if (originAccount == null)
            {
                return new TransferDto
                {
                    ReturnType = Utilities.ReturnType.NotFound,
                    WithdrawData = null
                };
            }
            else
            {
                var destinationAccount = _repository.Get(eventReqDto.Destination);

                if (destinationAccount == null)
                    return new TransferDto { ReturnType = Utilities.ReturnType.NotFound };

                //debit origin account
                originAccount.Balance -= eventReqDto.Amount;

                _repository.Update(originAccount);

                //credit origin account
                destinationAccount.Balance += eventReqDto.Amount;

                _repository.Update(destinationAccount);

                return new TransferDto
                {
                    ReturnType = Utilities.ReturnType.Created,
                    DepositData = new Data
                    {
                        Destination = new AccountDto
                        {
                            Id = destinationAccount.Id,
                            Balance = destinationAccount.Balance
                        }
                    },
                    WithdrawData = new WithDrawData
                    {
                        Origin = new AccountDto
                        {
                            Id = originAccount.Id,
                            Balance = originAccount.Balance
                        }
                    }
                };
            }
        }

        private TransferDto Transfer(EventReqDto eventReqDto)
        {
            var withdrwal = Withrawal(eventReqDto);

            if (withdrwal.ReturnType == Utilities.ReturnType.NotFound)
                return new TransferDto { ReturnType = withdrwal.ReturnType };

            var deposit = CreateAccountOrDeposit(eventReqDto);

            return new TransferDto
            {
                ReturnType = Utilities.ReturnType.Created,
                WithdrawData = withdrwal.WithdrawData,
                DepositData = deposit.DepositData
            };
        }
    }
}
