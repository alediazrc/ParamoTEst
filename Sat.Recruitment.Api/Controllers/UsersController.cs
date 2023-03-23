using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {

        public IUserService Service { get; set; }
        public UsersController(IUserService service)
        {
            Service = service;
        }

        [HttpPost]
        [Route("/create-user")]
        public async Task<Result> CreateUser([FromBody] User user)
        {

            return await Service.CreateUser(user);

        }
    }
}
