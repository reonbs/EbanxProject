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
                    return Created($"/event", account.data);
                case ReturnType.Ok:
                    return Ok(account.data);
                case ReturnType.NotFound:
                    return NotFound(0);
                default:
                    return NotFound(0);
            }
        }



        //[HttpGet("balance")]
        //public IActionResult Get(string account_id)
        //{
        //    _accountService.GetAccount(account_id);
        //    return Ok();
        //}
    }
}
