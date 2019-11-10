using System.Collections.Generic;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.Controllers
{
     [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
         private readonly IAccountService _accountService;
        public BookController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        // GET api/banking
        [HttpGet]
        public ActionResult<IEnumerable<Account>> Get()
        {
            return Ok(_accountService.GetAccounts());
        }

  
        
    }
}