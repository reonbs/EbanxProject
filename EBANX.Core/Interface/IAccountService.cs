using System;
using EBANX.Core.Dtos;

namespace EBANX.Core.Interface
{
    public interface IAccountService
    {
        TransactionDto EventService(EventReqDto eventReqDto);
        BalanceDto GetBalance(string accountId);
        void Reset();
    }
}
