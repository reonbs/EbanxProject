﻿using System;
using EBANX.Core.Dtos;

namespace EBANX.Core.Interface
{
    public interface IAccountService
    {
        DepositDto<object> CreateAccount(EventReqDto eventReqDto);
    }
}
