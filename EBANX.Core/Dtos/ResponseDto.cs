using System;
using EBANX.Core.Utilities;

namespace EBANX.Core.Dtos
{
    public class TransactionDto
    {
        public ReturnType ReturnType { get; set; }
        public dynamic Data { get; set; }
    }

    public class DepositDto : TransactionDto
    {
        public Data DepositData { get; set; }
    }

    public class WithdrwalDto : TransactionDto
    {
        public WithDrawData WithdrawData { get; set; }

    }

    public class TransferDto : TransactionDto
    {
        public Data DepositData { get; set; }
        public WithDrawData WithdrawData { get; set; }

    }

    public class BalanceDto
    {
        public ReturnType ReturnType { get; set; }
        public decimal Balance { get; set; }
    }

    public class Data
    {
        public AccountDto Destination { get; set; }
    }

    public class WithDrawData
    {
        public AccountDto Origin { get; set; }
    }
}
