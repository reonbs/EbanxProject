using System;
using EBANX.Core.Utilities;

namespace EBANX.Core.Dtos
{
    public class DepositDto<T> where T : class
    {
        public ReturnType ReturnType { get; set; }
        public Data data { get; set; }
    }

    public class WithdrwalDto<T> where T : class
    {
        public ReturnType ReturnType { get; set; }
        public object Origin { get; set; }
    }

    public class Data
    {
        public AccountDto Destination { get; set; }
    }
}
