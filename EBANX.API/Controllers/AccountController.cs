using System;
using EBANX.Core.Dtos;
using EBANX.Core.Interface;
using EBANX.Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EBANX.API.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("event")]
        public IActionResult Event([FromBody] EventReqDto eventReqDto)
        {
            var account = _accountService.EventService(eventReqDto);

            switch (account.ReturnType)
            {
                case ReturnType.Created:
                    return Created($"/event", account.Data);
                case ReturnType.Ok:
                    return Ok(account.Data);
                case ReturnType.NotFound:
                    return NotFound(0);
                default:
                    return NotFound(0);
            }
        }

        [HttpGet("balance")]
        public IActionResult Get(string account_id)
        {
            var account = _accountService.GetBalance(account_id);

            switch (account.ReturnType)
            {
                case ReturnType.Ok:
                    return Ok(account.Balance);
                case ReturnType.NotFound:
                    return NotFound(account.Balance);
                default:
                    return NotFound(0);
            }
        }

        [HttpPost("reset")]
        public IActionResult Reset()
        {
            _accountService.Reset();

            return Ok();
        }
    }
}
